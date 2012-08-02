var TaskModel = function () {
    var self = this;
    self.project = ko.observable();
    self.tasks = ko.observableArray([]);
};

var Task = function (id, name, description, createdDate, creator) {
    this.Id = id;
    this.Name = name;
    this.Description = description;
    this.CreatedDate = createdDate.toLocaleDateString();
    this.CreatedTime = createdDate.toLocaleTimeString();
    this.Creator = creator;
}