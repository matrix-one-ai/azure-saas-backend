using System;

namespace Icon.BaseManagement
{
    public class BaseBackendEventDto
    {
        public string BackendServiceName { get; set; }
        public string BackendMethodName { get; set; }
        public string BackendInputName { get; set; }
        public BaseModalType? ModalType { get; set; }

        public BaseBackendEventDto()
        {
        }
        public BaseBackendEventDto(string backendServiceName, string backendMethodName, BaseModalType? modalType = null)
        {
            BackendServiceName = backendServiceName;
            BackendMethodName = backendMethodName;
            ModalType = modalType;
        }
    }

}