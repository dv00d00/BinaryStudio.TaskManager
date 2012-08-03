var TaskModel = function () {
    var self = this;
    self.project = ko.observable();
    self.tasks = ko.observableArray([]);
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