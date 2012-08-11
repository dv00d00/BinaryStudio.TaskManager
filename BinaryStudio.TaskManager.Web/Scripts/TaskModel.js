var TaskModel = function () {
    var self = this;
    self.project = ko.observable();
    self.projectId = ko.observable();
    self.tasks = ko.observableArray([]);
    self.users = ko.observableArray([]);
    self.filter = ko.observable("all");
    self.userId = ko.observable();
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
        self.userId(task.AssigneeId);
    };
    self.allTasks = function () {
        $(".all_tasks").hide();
        self.filter("all");
    };
    self.tasksToShow = ko.computed(function () {
        if (self.filter() == 'all')
            return self.tasks();
        else if (self.filter() == 'user') {
            return ko.utils.arrayFilter(this.tasks(), function(task) {
                return task.AssigneeId == self.userId();
            });
        }

    }, this);
};