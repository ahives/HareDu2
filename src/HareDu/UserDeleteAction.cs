namespace HareDu
{
    public interface UserDeleteAction
    {
        /// <summary>
        /// Specify the user targeted for deletion.
        /// </summary>
        /// <param name="name"></param>
        void User(string name);
    }
}