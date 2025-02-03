using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class TaskPageModel : PageModel
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }
    }

    [BindProperty]
    public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

    [BindProperty]
    public string NewTask { get; set; }

    public void OnGet()
    {
        if (HttpContext.Session.GetString("Tasks") != null)
        {
            Tasks = System.Text.Json.JsonSerializer.Deserialize<List<TaskModel>>(HttpContext.Session.GetString("Tasks"));
        }
    }

    public IActionResult OnPost()
    {
        if (HttpContext.Session.GetString("Tasks") != null)
        {
            Tasks = System.Text.Json.JsonSerializer.Deserialize<List<TaskModel>>(HttpContext.Session.GetString("Tasks"));
        }

        if (!string.IsNullOrWhiteSpace(NewTask))
        {
            Tasks.Add(new TaskModel { Id = Tasks.Count + 1, Text = NewTask, Completed = false });
        }

        HttpContext.Session.SetString("Tasks", System.Text.Json.JsonSerializer.Serialize(Tasks));

        return RedirectToPage();
    }

    public IActionResult OnPostToggle(int taskId)
    {
        if (HttpContext.Session.GetString("Tasks") != null)
        {
            Tasks = System.Text.Json.JsonSerializer.Deserialize<List<TaskModel>>(HttpContext.Session.GetString("Tasks"));
        }

        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.Completed = !task.Completed;
        }

        HttpContext.Session.SetString("Tasks", System.Text.Json.JsonSerializer.Serialize(Tasks));

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int taskId)
    {
        if (HttpContext.Session.GetString("Tasks") != null)
        {
            Tasks = System.Text.Json.JsonSerializer.Deserialize<List<TaskModel>>(HttpContext.Session.GetString("Tasks"));
        }

        Tasks = Tasks.Where(t => t.Id != taskId).ToList();
        HttpContext.Session.SetString("Tasks", System.Text.Json.JsonSerializer.Serialize(Tasks));

        return RedirectToPage();
    }
}
