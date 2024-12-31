using System;
using System.Collections.Generic;
using Abp.UI;

namespace Icon.BaseManagement
{
    public static class BaseModalButtonFactory
    {

        public static BaseModalFooterButtonDto GeneratePrompt(BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(GeneratePrompt),
            Text = "Modal.FooterButton.GeneratePrompt",
            Icon = BaseIconOptions.Generate,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "switchModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Left,
            SwitchModal = true,
            SwitchModalEvent = switchModalEvent
        };

        public static BaseModalFooterButtonDto TestAutoTweet(BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(PostTweet),
            Text = "Modal.FooterButton.PostTweet",
            Icon = BaseIconOptions.PostTweet,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Left,
            BackendEvent = backendEvent
        };

        public static BaseModalFooterButtonDto TestAutoTweetPrompt(BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(PostTweet),
            Text = "Modal.FooterButton.TestTweetPrompt",
            Icon = BaseIconOptions.Generate,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Left,
            BackendEvent = backendEvent
        };

        public static BaseModalFooterButtonDto TestAutoTweetPromptInput(BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(PostTweet),
            Text = "Modal.FooterButton.TestTweetPromptInput",
            Icon = BaseIconOptions.Generate,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Left,
            BackendEvent = backendEvent
        };

        public static BaseModalFooterButtonDto PostTweet(BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(PostTweet),
            Text = "Modal.FooterButton.PostTweet",
            Icon = BaseIconOptions.PostTweet,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "switchModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Left,
            SwitchModal = true,
            SwitchModalEvent = switchModalEvent
        };

        public static BaseModalFooterButtonDto Back(BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(Back),
            Text = "Modal.FooterButton.Back",
            Icon = BaseIconOptions.Back,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "back",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Left,
            SwitchModal = true,
            SwitchModalEvent = switchModalEvent
        };

        public static BaseModalFooterButtonDto SwitchToEditModal(string entity, BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto()
        {
            Type = nameof(SwitchToEditModal),
            Text = "Modal.FooterButton.Edit" + entity,
            Icon = BaseIconOptions.Edit,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            SwitchModal = true,
            Event = "switchModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            SwitchModalEvent = switchModalEvent
        };

        public static BaseModalFooterButtonDto SwitchToHandleModal(string entity, BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(SwitchToHandleModal),
            Text = "Modal.FooterButton.Handle" + entity,
            Icon = BaseIconOptions.Handle,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            Event = "switchModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            SwitchModal = true,
            SwitchModalEvent = switchModalEvent
        };

        public static BaseModalFooterButtonDto SwitchToCancelModal(string entity, BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(SwitchToCancelModal),
            Text = "Modal.FooterButton.Cancel" + entity,
            Icon = BaseIconOptions.Cancel,
            Class = "btn btn-light-danger btn-outline-danger fw-bold",
            Event = "switchModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            SwitchModal = true,
            SwitchModalEvent = switchModalEvent
        };

        public static BaseModalFooterButtonDto ClaimAndSwitchToHandle(string entity, BaseBackendEventDto backendEvent, BaseBackendEventDto switchModalEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(ClaimAndSwitchToHandle),
            Text = "Modal.FooterButton.ChangeClaim" + entity,
            Icon = BaseIconOptions.ChangeClaim,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            Event = "submitAndSwitchModal",
            BackendEvent = backendEvent,
            SwitchModalEvent = switchModalEvent,
            IsVisible = true,
            SwitchModal = true,
            Alignment = BaseAlignmentOptions.Right
        };

        public static BaseModalFooterButtonDto Save(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(Save),
            Text = "Modal.FooterButton.Save" + entity,
            Icon = BaseIconOptions.Save,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            BackendEvent = backendEvent
        };

        public static BaseModalFooterButtonDto Submit(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(Submit),
            Text = "Modal.FooterButton.Submit" + entity,
            Icon = BaseIconOptions.Save,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            BackendEvent = backendEvent,
        };

        public static BaseModalFooterButtonDto ReSubmit(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(ReSubmit),
            Text = "Modal.FooterButton.ChangeReSubmit" + entity,
            Icon = BaseIconOptions.ChangeSubmit,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            BackendEvent = backendEvent,
        };

        public static BaseModalFooterButtonDto ChangeProcessed(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(ChangeProcessed),
            Text = "Modal.FooterButton.ChangeProcessed" + entity,
            Icon = BaseIconOptions.ChangeProcessed,
            Class = "btn btn-light-success btn-outline-success fw-bold",
            Event = "submitModal",
            BackendEvent = backendEvent,
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right
        };
        public static BaseModalFooterButtonDto ChangeRejectCanEdit(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(ChangeRejectCanEdit),
            Text = "Modal.FooterButton.ChangeRejectCanEdit" + entity,
            Icon = BaseIconOptions.ChangeRejectCanEdit,
            Class = "btn btn-light-info btn-outline-info fw-bold",
            Event = "submitModal",
            BackendEvent = backendEvent,
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right
        };

        public static BaseModalFooterButtonDto ChangeRejectCantEdit(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(ChangeRejectCantEdit),
            Text = "Modal.FooterButton.ChangeRejectCantEdit" + entity,
            Icon = BaseIconOptions.ChangeRejectCantEdit,
            Class = "btn btn-light-danger btn-outline-danger fw-bold",
            Event = "submitModal",
            BackendEvent = backendEvent,
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right
        };
        public static BaseModalFooterButtonDto Cancel(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(Cancel),
            Text = "Modal.FooterButton.Cancel" + entity,
            Icon = BaseIconOptions.Cancel,
            Class = "btn btn-light-danger btn-outline-danger fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            BackendEvent = backendEvent
        };


        public static BaseModalFooterButtonDto CancelConfirm(string entity, BaseBackendEventDto backendEvent) => new BaseModalFooterButtonDto
        {
            Type = nameof(CancelConfirm),
            Text = "Modal.FooterButton.CancelConfirm" + entity,
            Icon = BaseIconOptions.Cancel,
            Class = "btn btn-light-danger btn-outline-danger fw-bold",
            Event = "submitModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right,
            BackendEvent = backendEvent
        };

        public static BaseModalFooterButtonDto CloseModal => new BaseModalFooterButtonDto
        {
            Type = nameof(CloseModal),
            Text = "Modal.FooterButton.Cancel",
            Icon = BaseIconOptions.Close,
            Class = "btn btn-light-primary btn-outline-primary fw-bold",
            Event = "closeModal",
            IsVisible = true,
            Alignment = BaseAlignmentOptions.Right
        };

    }
}
