
namespace Icon.BaseManagement
{

    // Todo .. change to builder
    public class BasePageDto
    {
        public string PageTitle { get; set; }
        public string PageIcon { get; set; }
        public bool PageHeaderShow { get; set; }
        public BaseListDto List { get; set; }



    }

    public enum BasePageType
    {
        Memories,
        CharacterPersonas,
        Characters
    }
}

