var TaskModel = function (tasks) {
    var self = this;
    self.contacts = ko.observableArray(ko.utils.arrayMap(tasks, function (task) {
        return {
            id: task.Id,
            name: task.Name,
            description: task.Description,
            priority: task.Priority
        };
    }));
}