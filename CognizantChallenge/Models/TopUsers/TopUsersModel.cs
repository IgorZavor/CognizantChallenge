using System.Collections.Generic;

namespace CognizantChallenge.Models.TopUsers
{
    public class TopUsersModel
    {
        public IEnumerable<TopUser> TopUsers{ get; set; }
    } 
    
    public class TopUser
    {
        public string Name { get; set; }
        public int Scores { get; set; }
        public string Tasks { get; set; }
    }
}