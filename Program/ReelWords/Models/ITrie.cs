namespace ReelWords.Models
{
    public interface ITrie
    {
        bool Search(string word); 
        void Insert(string word); 
        void Delete(string word);
    }
}