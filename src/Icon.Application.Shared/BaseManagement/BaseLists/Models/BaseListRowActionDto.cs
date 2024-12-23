using System;

namespace Icon.BaseManagement
{
    public class BaseListRowActionDto
    {
        // dont use subclasses for this dto
        public string Icon { get; set; }
        public string Class { get; set; }
        public string Tooltip { get; set; }
        public string Event { get; set; }
        public string EntityIdPath { get; set; }
        public string DisabledPath { get; set; }
        public BaseListRowActionType ActionType { get; set; }
        public bool OpenModal { get; set; }
        public BaseModalType ModalType { get; set; }

        public Guid? EntityId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? LocationId { get; set; }

        public string BackendServiceName { get; set; }
        public string BackendMethodName { get; set; }
        public string BackendInputName { get; set; }


        public BaseListRowActionDto(
            BaseListRowActionType actionType, string entityIdPath, string tooltip, BaseModalType modalType)
        {
            ActionType = actionType;
            EntityIdPath = entityIdPath;
            OpenModal = true;
            ModalType = modalType;
            Tooltip = tooltip;

            SetActionSettings();
        }

        public BaseListRowActionDto(
            BaseListRowActionType actionType, BaseBackendEventDto backendEvent, string entityIdPath, string tooltip, BaseModalType modalType)
        {
            ActionType = actionType;

            BackendServiceName = backendEvent.BackendServiceName;
            BackendMethodName = backendEvent.BackendMethodName;
            BackendInputName = backendEvent.BackendInputName;

            EntityIdPath = entityIdPath;
            OpenModal = true;
            ModalType = modalType;
            Tooltip = tooltip;

            SetActionSettings();
        }

        public BaseListRowActionDto(
            BaseListRowActionType actionType, string entityIdPath, string eventName, string tooltip)
        {
            ActionType = actionType;
            EntityIdPath = entityIdPath;
            Event = eventName;
            Tooltip = tooltip;

            SetActionSettings();
        }

        private void SetActionSettings()
        {
            if (OpenModal)
            {
                Event = "app.baseModal.rowAction";
            }

            if (ActionType == BaseListRowActionType.Open)
            {
                Icon = BaseIconOptions.Open;
                Class = "btn btn-sm btn-outline-primary btn-icon btn-light-primary";
                DisabledPath = "rowSettings.canOpen";
            }
            else if (ActionType == BaseListRowActionType.Edit)
            {
                Icon = BaseIconOptions.Edit;
                Class = "btn btn-sm btn-outline-info btn-icon btn-light-info";
                DisabledPath = "rowSettings.canEdit";
            }
            else if (ActionType == BaseListRowActionType.Delete)
            {
                Icon = BaseIconOptions.Delete;
                Class = "btn btn-sm btn-outline-danger btn-icon btn-light-danger";
                DisabledPath = "rowSettings.canDelete";
            }
            else if (ActionType == BaseListRowActionType.Cancel)
            {
                Icon = BaseIconOptions.Cancel;
                Class = "btn btn-sm btn-outline-danger btn-icon btn-light-danger";
                DisabledPath = "rowSettings.canCancel";
            }
            else if (ActionType == BaseListRowActionType.CreateClientBlockade)
            {
                Icon = BaseIconOptions.Blockade;
                Class = "btn btn-sm btn-outline-info btn-icon btn-light-info";
                DisabledPath = "rowSettings.canCreateClientBlockade";
            }
            else if (ActionType == BaseListRowActionType.Handle)
            {
                Icon = BaseIconOptions.Handle;
                Class = "btn btn-sm btn-outline-info btn-icon btn-light-info";
                DisabledPath = "rowSettings.canHandle";
            }
        }
    }

    public class BaseListRowSettingsDto
    {
        public bool CanOpen { get; set; }
        public bool CanEdit { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }
        public bool CanHandle { get; set; }
        public bool CanCreateClientBlockade { get; set; }
    }


    public enum BaseListRowActionType
    {
        CreateClientBlockade,
        Open,
        Edit,
        Cancel,
        Delete,
        Handle,
    }


}
