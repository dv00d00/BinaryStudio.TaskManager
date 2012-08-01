var TaskModel = function () {
    var self = this;
    self.tasks = ko.observableArray([]);
};

var Task = function (id, name, description, createdDate, creator) {
    this.Id = id;
    this.Name = name;
    this.Description = description;
    this.CreatedDate = createdDate;
    this.Creator = creator;
}