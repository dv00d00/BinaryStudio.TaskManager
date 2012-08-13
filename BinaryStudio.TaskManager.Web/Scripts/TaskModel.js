var TaskModel = function () {
    var self = this;
    self.project = ko.observable();
    self.projectId = ko.observable();
    self.tasks = ko.observableArray([]);
    self.users = ko.observableArray([]);
    self.filter = ko.observable("all");
    self.data = new Object(
    {
        userId: ko.observable(),
        name: ko.observable()
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

    self.filterByAssignee = function (task) {
        $(".all_tasks").show();
        self.filter("user");
        self.data.userId(task.AssigneeId);
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

    self.allTasks = function () {
        $(".all_tasks").hide();
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
                return ko.utils.arrayFilter(this.tasks(), function (task) {
                    return task.Name.toLowerCase().indexOf(name) != -1;
                });
            }
        }
    }, this);
    /*self.addTask = function() {
        
    };*/
};