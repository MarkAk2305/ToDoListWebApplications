using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoListWebApplications.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Web;

    public class ToDo
    {
        public long ToDoID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please Enter Task Name.")]
        public string ToDoName { get; set; }

        [DisplayName("Description")]
        [StringLength(maximumLength: 500, ErrorMessage = "Please Provide a Task Description of Length 500 Letters Max")]
        public string ToDoDescription { get; set; }

        [DisplayName("Creation Date")]
        //[Required(ErrorMessage = "Please Select Due Date.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? CreationDate { get; set; }

        [DisplayName("Due Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Please Choose Priority.")]
        [DefaultValue("Low")]
        public string Priority { get; set; }
        public bool Status { get; set; }

        [DisplayName("Image")]
        public string Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}