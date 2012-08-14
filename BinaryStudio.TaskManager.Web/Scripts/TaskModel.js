var TaskModel = function () {
    var self = this;
    self.project = ko.observable();
    self.projectId = ko.observable();
    self.tasks = ko.observableArray([]);
    self.users = ko.observableArray([]);
    self.filter = ko.observable("all");
    self.userFilter = ko.observable("all");
    self.data = new Object(
    {
        userId: ko.observable(),
        name: ko.observable(),
        user: ko.observable()
    });

    self.isProject = function () {
        return self.projectId() != -1;
    };

    self.sortByPriority = function () {
        self.tasks.sort(function (left, right) {
            return left.Priority == right.Priority ? 0 : (left.Priority > right.Priority ? -1 : 1);
        });
    };

    self.sortByName = function () {
        self.tasks.sort(function (left, right) {
            return left.Name == right.Name ? 0 : (left.Name < right.Name ? -1 : 1);
        });
    };

    self.sortByAssignee = function () {
        self.tasks.sort(function (left, right) {
            return left.AssigneeId == right.AssigneeId ? 0 : (left.AssigneeId < right.AssigneeId ? -1 : 1);
        });
    };

    self.sortByDate = function () {
        self.tasks.sort(function (left, right) {
            return left.CompareDate == right.CompareDate ? 0 :
                (left.CompareDate < right.CompareDate ? -1 : 1);
        });
    };

    self.filterByAssignee = function (task) {
        var user_li = $(".user_list li[data-id='" + task.AssigneeId + "']");
        $(".all_tasks").show();
        $("#content h3 span").text(task.Assignee);
        $('#content h3').show();
        self.filter("user");
        self.data.userId(task.AssigneeId);
        $(".add_task_assignee_photo").html("");
        user_li.children("img").clone().appendTo($(".add_task_assignee_photo"));
        $(".add_task_assignee").attr("data-id", task.AssigneeId);
        $(".add_task_assignee_name span").html(user_li.children("span").text());
    };

    self.filterTasksByName = function () {
        var search_val = $("#filterByName").val();
        if (search_val != '') {
            $(".all_tasks").show();
            self.data.name(search_val);
            self.filter("name");
        }
        else {
            self.allTasks();
        }
    };

    self.filterUsersByName = function () {
        var search_val = $(".user_list_input").val();
        if (search_val != '') {
            self.data.user(search_val);
            self.userFilter("name");
        }
        else {
            self.allUsers();
        }
    };
    self.allTasks = function () {
        if (self.filter() == "user") {
            $(".add_task_assignee_photo").html("");
            $(".add_task_assignee span").text("No one");
            $(".add_task_assignee").attr("data-id", "");
        }
        $(".all_tasks").hide();
        $('#content h3').hide();
        var filterByName = $("#filterByName");
        filterByName.val("");
        self.data.name("");
        self.filter("all");
        if (self.tasks().length == 0) {
            filterByName.prop("disabled", "true");
        }
        else {
            filterByName.removeAttr("disabled");
        }
    };

    self.allUsers = function () {
        var filterByName = $(".user_list_input");
        filterByName.val("");
        self.data.user("");
        self.userFilter("all");
    };
    
    self.tasksToShow = ko.computed(function () {
        if (self.filter() == 'all')
            return self.tasks();
        else if (self.filter() == 'user') {
            return ko.utils.arrayFilter(this.tasks(), function (task) {
                return task.AssigneeId == self.data.userId();
            });
        }
        else if ((self.filter() == 'name')) {
            var name = this.data.name().length == 0 ? null : this.data.name().toLowerCase();
            if (!name) {
                return this.tasks();
            } else {
                return ko.utils.arrayFilter(self.tasks(), function (task) {
                    return ((task.Name.toLowerCase().indexOf(name) != -1) || (task.Description.toLowerCase().indexOf(name) != -1));
                });
            }
        }
    }, this);
    self.usersToShow = ko.computed(function () {
        if (self.userFilter() == 'all')
            return self.users();
        else if (self.userFilter() == 'name') {
            var name = self.data.user().length == 0 ? null : self.data.user().toLowerCase();
            return ko.utils.arrayFilter(self.users(), function (user) {
                return (user.Name.toLowerCase().indexOf(name) != -1);
            });
            
        }
    });
};