using System;

namespace MyTaskManager.Shared.Models
{
    public class TaskItem
    {
        public int Id { get; set; }                     // Primary key
        public required string Title { get; set; }               // Short title of task
        
        public TaskStatus Status { get; set; }          // Enum value
        public DateTime CreatedAt { get; set; }         // When added
        public DateTime? DueDate { get; set; }          // Optional
        public required string OwnerUserId { get; set; }         // Who owns it (links to user)
    }
}
