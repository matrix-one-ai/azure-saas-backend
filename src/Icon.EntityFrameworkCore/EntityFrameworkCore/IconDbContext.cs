﻿using System.Collections.Generic;
using System.Text.Json;
using Abp.OpenIddict.Applications;
using Abp.OpenIddict.Authorizations;
using Abp.OpenIddict.EntityFrameworkCore;
using Abp.OpenIddict.Scopes;
using Abp.OpenIddict.Tokens;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Icon.Authorization.Delegation;
using Icon.Authorization.Roles;
using Icon.Authorization.Users;
using Icon.Chat;
using Icon.Editions;
using Icon.ExtraProperties;
using Icon.Friendships;
using Icon.MultiTenancy;
using Icon.MultiTenancy.Accounting;
using Icon.MultiTenancy.Payments;
using Icon.Storage;
using Icon.Matrix;
using Stripe;
using Icon.Matrix.Models;
using Icon.Matrix.Enums;

namespace Icon.EntityFrameworkCore
{
    public class IconDbContext : AbpZeroDbContext<Tenant, Role, User, IconDbContext>, IOpenIddictDbContext
    {
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<CharacterBio> CharacterBios { get; set; }

        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<PersonaPlatform> PersonaPlatforms { get; set; }

        public virtual DbSet<CharacterTopic> CharacterTopics { get; set; }
        public virtual DbSet<CharacterPlatform> CharacterPlatforms { get; set; }
        public virtual DbSet<CharacterPersona> CharacterPersonas { get; set; }
        public virtual DbSet<CharacterPersonaTwitterRank> CharacterPersonaTwitterRanks { get; set; }
        public virtual DbSet<CharacterPersonaTwitterProfile> CharacterPersonaTwitterProfiles { get; set; }

        public virtual DbSet<Memory> Memories { get; set; }
        public virtual DbSet<MemoryType> MemoryTypes { get; set; }
        public virtual DbSet<MemoryTopic> MemoryTopics { get; set; }
        public virtual DbSet<MemoryStatsTwitter> MemoryStatsTwitters { get; set; }

        public virtual DbSet<MemoryPrompt> MemoryPrompts { get; set; }
        // public virtual DbSet<MemoryAction> MemoryActions { get; set; }
        public virtual DbSet<MemoryProcess> MemoryProcesses { get; set; }
        public virtual DbSet<MemoryProcessStep> MemoryProcessSteps { get; set; }
        public virtual DbSet<MemoryProcessLog> MemoryProcessLogs { get; set; }

        public virtual DbSet<Agent> Agents { get; set; }

        public virtual DbSet<TwitterImportTweet> TwitterImportTweets { get; set; }
        public virtual DbSet<TwitterImportTask> TwitterImportTasks { get; set; }
        public virtual DbSet<TwitterImportLog> TwitterImportLogs { get; set; }


        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<OpenIddictApplication> Applications { get; }

        public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

        public virtual DbSet<OpenIddictScope> Scopes { get; }

        public virtual DbSet<OpenIddictToken> Tokens { get; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

        public virtual DbSet<MultiTenancy.Accounting.Invoice> Invoices { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public IconDbContext(DbContextOptions<IconDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<PersonaPlatform>()
            //     .HasOne(pp => pp.Persona)
            //     .WithMany(p => p.Platforms)
            //     .HasForeignKey(pp => pp.PersonaId)
            //     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Memory>()
                .HasOne(m => m.Character)
                .WithMany()
                .HasForeignKey(m => m.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Memory>()
                .HasOne(m => m.CharacterBio)
                .WithMany()
                .HasForeignKey(m => m.CharacterBioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Memory>()
                .HasOne(m => m.MemoryType)
                .WithMany()
                .HasForeignKey(m => m.MemoryTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Memory>()
                .HasOne(m => m.MemoryProcess)
                .WithOne(mp => mp.Memory)
                .HasForeignKey<MemoryProcess>(mp => mp.MemoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemoryProcess>()
                .HasMany(mp => mp.Steps)
                .WithOne(mps => mps.MemoryProcess)
                .HasForeignKey(mps => mps.MemoryProcessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemoryProcessLog>()
                .HasOne(mpl => mpl.MemoryProcess)
                .WithMany(mp => mp.Logs)
                .HasForeignKey(mpl => mpl.MemoryProcessId)
                .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<Memory>()
            //     .HasOne(m => m.MemoryStatsTwitter)
            //     .WithOne(ms => ms.Memory)
            //     .HasForeignKey<MemoryStatsTwitter>(ms => ms.MemoryId)
            //     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Memory>()
                .HasIndex(m => m.PlatformInteractionDate)
                .HasDatabaseName("IX_Memory_PlatformInteractionDate");

            // modelBuilder.Entity<CharacterBio>()
            //     .HasOne(cb => cb.Character)
            //     .WithMany()
            //     .HasForeignKey(cb => cb.CharacterId)
            //     .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

            modelBuilder.Entity<SubscriptionPayment>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigureOpenIddict();
        }
    }
}