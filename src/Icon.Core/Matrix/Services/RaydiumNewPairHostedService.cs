using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Icon.Configuration;
using Icon.Matrix.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Icon.Matrix.Raydium
{
    public class RaydiumNewPairHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<RaydiumNewPairHostedService> _logger;
        private readonly IRepository<RaydiumPair, Guid> _raydiumPairRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConfigurationRoot _configuration;

        private ClientWebSocket _webSocket;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _backgroundTask;

        public RaydiumNewPairHostedService(
            ILogger<RaydiumNewPairHostedService> logger,
            IRepository<RaydiumPair, Guid> raydiumPairRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IAppConfigurationAccessor appConfigurationAccessor
        )
        {
            _logger = logger;
            _raydiumPairRepository = raydiumPairRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Start the background loop in a dedicated Task so we don’t block.
            _backgroundTask = Task.Run(() => ConnectAndSubscribeLoop(_cancellationTokenSource.Token),
                                       _cancellationTokenSource.Token);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Signal the background loop to stop
                _cancellationTokenSource.Cancel();
            }
            finally
            {
                // Attempt a graceful close if the WebSocket is open
                if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        await _webSocket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Stopping the service",
                            cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while closing WebSocket.");
                    }
                }
            }

            // Wait until the background task is done
            if (_backgroundTask != null)
            {
                await _backgroundTask;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _webSocket?.Dispose();
            _cancellationTokenSource?.Dispose();
        }

        private async Task ConnectAndSubscribeLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Create a fresh WebSocket each time we try to connect
                _webSocket = new ClientWebSocket();

                try
                {
                    await ConnectAndSubscribe(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Graceful stop
                    _logger.LogInformation("WebSocket subscription is stopping due to cancellation.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in the WebSocket connection. Will attempt to reconnect.");
                }
                finally
                {
                    // Ensure we close and dispose the WebSocket when done
                    if (_webSocket != null)
                    {
                        _webSocket.Dispose();
                        _webSocket = null;
                    }
                }

                // If we’re still not cancelled, wait 1 minute before reconnecting
                if (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Waiting 1 minute before reconnecting...");
                    try
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        // If cancellation is requested during the delay, just exit.
                        _logger.LogInformation("Reconnect delay was cancelled.");
                        break;
                    }
                }
            }
        }

        private async Task ConnectAndSubscribe(CancellationToken cancellationToken)
        {
            var apiKey = _configuration["MatrixRaydiumStream:ApiKey"];
            _webSocket.Options.SetRequestHeader("X-API-KEY", apiKey);

            var uri = new Uri("wss://api.solanastreaming.com/");
            await _webSocket.ConnectAsync(uri, cancellationToken);

            _logger.LogInformation("Connected to WebSocket.");

            // Once connected, send the subscription request
            var subscribeMessage = new
            {
                jsonrpc = "2.0",
                id = 1,
                method = "newPairSubscribe"
            };

            var subscribeJson = JsonSerializer.Serialize(subscribeMessage);
            var bytes = System.Text.Encoding.UTF8.GetBytes(subscribeJson);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken
            );

            _logger.LogInformation("Subscription message sent: {Message}", subscribeJson);

            // Continuously receive messages
            var buffer = new byte[4096];
            while (!cancellationToken.IsCancellationRequested
                   && _webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                using var ms = new System.IO.MemoryStream();

                do
                {
                    result = await _webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _logger.LogWarning("Received Close frame from WebSocket");
                        await _webSocket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Closing",
                            cancellationToken);
                        return; // This triggers the reconnect loop.
                    }

                    ms.Write(buffer, 0, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, System.IO.SeekOrigin.Begin);
                var messageString = Encoding.UTF8.GetString(ms.ToArray());

                // Log or process the incoming message
                _logger.LogInformation("Received WebSocket message: {Message}", messageString);

                // Attempt to parse the message
                try
                {
                    var wrapper = JsonSerializer.Deserialize<RaydiumResponseWrapper>(messageString);
                    if (wrapper?.Params != null)
                    {
                        var notification = wrapper.Params;
                        using (var uow = _unitOfWorkManager.Begin())
                        {
                            var entity = RaydiumModelConverter.ToRaydiumPairEntity(notification);
                            await _raydiumPairRepository.InsertAsync(entity);
                            await _unitOfWorkManager.Current.SaveChangesAsync();
                            uow.Complete();
                        }
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error parsing JSON from WebSocket message.");
                }
            }
        }
    }

    public static class RaydiumModelConverter
    {
        public static RaydiumPair ToRaydiumPairEntity(RaydiumNewPairNotification notification)
        {
            if (notification == null)
                return null;

            var entity = new RaydiumPair
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,

                Slot = notification.Slot,
                Signature = notification.Signature,
                BlockTime = notification.BlockTime,
                SourceExchange = notification.Pair?.SourceExchange,
                AmmAccount = notification.Pair?.AmmAccount,

                BaseTokenAccount = notification.Pair?.BaseToken?.Account,
                BaseTokenDecimals = notification.Pair?.BaseToken?.Info?.Decimals,
                BaseTokenSupply = notification.Pair?.BaseToken?.Info?.Supply,
                BaseTokenName = notification.Pair?.BaseToken?.Info?.Metadata?.Name,
                BaseTokenSymbol = notification.Pair?.BaseToken?.Info?.Metadata?.Symbol,
                BaseTokenLogo = notification.Pair?.BaseToken?.Info?.Metadata?.Logo,
                BaseTokenLiquidityAdded = notification.Pair?.BaseTokenLiquidityAdded,

                QuoteTokenAccount = notification.Pair?.QuoteToken?.Account,
                QuoteTokenLiquidityAdded = notification.Pair?.QuoteTokenLiquidityAdded
            };

            return entity;
        }
    }
}
