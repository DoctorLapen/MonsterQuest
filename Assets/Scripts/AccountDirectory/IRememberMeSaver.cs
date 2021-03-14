namespace MonsterQuest
{
    public interface IRememberMeSaver
    {
        void Save(RememberMeInfo info);
        RememberMeInfo Load();
        void Delete();
    }
}