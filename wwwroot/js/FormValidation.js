$(document).ready(function() {
  $("form#add-task").submit(function(event) {
    var taskName = $("#task-name").val();
    var taskDescription = $("#task-description").val();
    if (taskName === "") {
      
      event.preventDefault();
    }
    if (taskDescription === "") {
      event.preventDefault();
    }
  });
});
