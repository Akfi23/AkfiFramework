namespace _Source.Code._AKFramework.AKTags.Runtime
{
    public interface IAKTagsService : IAKService
    {
        AKTag GetAKTagByName(string name);
        public AKTag GetAKTagByID(string id);
        AKTagsGroup GetAKTagsGroup(AKTag tag); 
    }
}