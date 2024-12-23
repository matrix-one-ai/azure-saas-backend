using System.Collections.Generic;

namespace Icon.BaseManagement
{
    public class BaseModalDto
    {
        public string ModalTitle { get; set; }
        public string ModalEntityName { get; set; }
        public string ModalIcon { get; set; }
        public string ModalSize { get; set; }
        public BaseFormDto Form { get; set; }
        public List<BaseModalFooterButtonDto> FooterButtons { get; set; }
        public string EventOnClose { get; set; }

        public object PromptContextJson { get; set; }
        public object PrompResponseJson { get; set; }

        public void RemoveFooterButtons(List<string> types)
        {
            FooterButtons.RemoveAll(x => types.Contains(x.Type));
        }
    }

    public class BaseModalSubmittedDto
    {
        public string SuccessMessage { get; set; }
        public BaseListEvent RefreshListEvent { get; set; }
    }

    public enum BaseModalType
    {
        PlaceHolder,
        ModalNew,
        ModalView,
        ModalEdit,
        ModalDelete,


        // LocationTripModalView,
        // LocationTripModalCancel,

        // LocationBlockadeModalNew,
        // LocationBlockadeModalNewFromClient,
        // LocationBlockadeModalView,
        // LocationBlockadeModalEdit,
        // LocationBlockadeModalCancel,

        // LocationScheduleModalView,

        // ChangeRequestLocationTripModalNew,
        // ChangeRequestLocationTripModalEdit,
        // ChangeRequestLocationTripModalView,
        // ChangeRequestLocationTripModalHandle,
        // ChangeRequestLocationTripModalCancel,

        // ChangeRequestClientScheduleModalNew,
        // ChangeRequestClientScheduleModalEdit,
        // ChangeRequestClientScheduleModalView,
        // ChangeRequestClientScheduleModalHandle,
        // ChangeRequestClientScheduleModalCancel,

        // ChangeRequestRegisterNewClientModalNew,
        // ChangeRequestRegisterNewClientModalEdit,
        // ChangeRequestRegisterNewClientModalView,
        // ChangeRequestRegisterNewClientModalHandle,
        // ChangeRequestRegisterNewClientModalCancel,

        // ChangeRequestGeneralChangeModalNew,
        // ChangeRequestGeneralChangeModalEdit,
        // ChangeRequestGeneralChangeModalView,
        // ChangeRequestGeneralChangeModalHandle,
        // ChangeRequestGeneralChangeModalCancel,

    }

    public class BaseModalSizeOptions
    {
        public static readonly string Small = "modal-dialog modal-sm";
        public static readonly string Medium = "modal-dialog modal-md";
        public static readonly string Large = "modal-dialog modal-lg";
        public static readonly string ExtraLarge = "modal-dialog modal-xl";
    }

    public class BaseModalFooterButtonDto
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Class { get; set; }
        public string Alignment { get; set; }
        public string Event { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsVisible { get; set; }
        public bool SwitchModal { get; set; }
        public BaseBackendEventDto BackendEvent { get; set; }
        public BaseBackendEventDto SwitchModalEvent { get; set; }
        // public BaseModalType ModalType { get; set; }

    }

}