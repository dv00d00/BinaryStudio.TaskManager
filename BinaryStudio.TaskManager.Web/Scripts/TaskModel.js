var TaskModel = function () {
    var self = this;
    self.project = ko.observable();
    self.tasks = ko.observableArray([]);
    self.sortByPriority = function() {
        self.tasks.sort(function(left, right) {
            return left.Priority == right.Priority ? 0 : (left.Priority < right.Priority ? -1 : 1);
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
};

var Task = function (id, name, description, createdDate, creator, priority, assigneeId, assignee, assigneePhoto) {
    this.Id = id;
    this.Name = name;
    this.Description = description;
    this.CreatedDate = createdDate.toLocaleDateString();
    this.CreatedTime = createdDate.toLocaleTimeString();
    this.Creator = creator;
    this.Priority = priority;
    this.AssigneeId = assigneeId;
    this.Assignee = assignee;
    this.AssigneePhoto = assigneePhoto;
}