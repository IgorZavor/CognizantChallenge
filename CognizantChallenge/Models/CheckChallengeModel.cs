namespace CognizantChallenge.Models
{
    public class CheckChallengeModel
    {
        public int UserId { get; set; }
        public int ChallengeId { get; set; }
        public string Script { get; set; }
        public string Language { get; set; }
    }
}