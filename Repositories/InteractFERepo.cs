using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class InteractFERepo: IInteractFE
    {
        private readonly dataContext db;

        public InteractFERepo(dataContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateQuestion(Question question)
        {
            try
            {
                question.Create_at = DateTime.Now;
                question.Update_at = DateTime.Now;
                db.Questions.Add(question);
                var rs = await db.SaveChangesAsync();
                if (rs > 0)
                {
                    return 200;
                }
                else
                {
                    return 201;
                }
            }catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CreateRate(ListRate list)
        {
            try
            {
                foreach (var item in list.Rates)
                {
                    item.Create_at = DateTime.Now;
                    item.Update_at = DateTime.Now;
                    db.Rates.Add(item);
                   
                    
                }
              var rs =  await db.SaveChangesAsync();
                if(rs > 0) {
                 
                    return 200;
                }
                return 201;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
