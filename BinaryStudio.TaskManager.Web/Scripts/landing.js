var modelData = new TaskModel();
var toDashboard = function (element, where) {
    var num = element.parent("div").attr("data-id");
    if (where == "here"){
        $("*").css("cursor", "progress");
        location.href = "/Project/Project/" + num;
    }
    else
        window.open("/Project/Project/" + num);
};
$(function () {
    /*********** To dashboard clicks handling ************/
    $(document).on("dblclick", ".project_list .project_name", function () {
        toDashboard($(this), "here");
    });
    $(document).on("mousedown", ".project_list .project_name", function (e) {
        if (e.which == 2) {
            toDashboard($(this), "there");
        }
    });

    /************** Assign menu **************/

    $(document).on("click", ".img_holder", function (e) {
        var top = e.pageY + 25;
        var left = e.pageX - 225;
        $(".user_list_holder").css({
            "display": "block",
            "top": top,
            "left": left
        });
        $(document).keyup(function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            if (code == 27) {
                $(".user_list_holder").hide();
            }
            $("user_list_holder").off();
        });
        $(".user_list_close").click(function () {
            $(".user_list_holder").hide();
            $(".user_list_close").off("click");
        });
        var taskId = $(this).parent("div").parent("div").attr("data-id");
        $(document).on("click", ".user_list li", function () {
            var userId = $(this).attr('data-id');
            $.ajax({
                'data': { 'taskId': taskId,
                    'senderId': -1,
                    'receiverId': userId
                },
                'dataType': 'JSON',
                'type': 'POST',
                'url': '/HumanTasks/MoveTask',
                success: function () {
                    location.reload(true);
                }
            });
            $(".user_list li").off();
        });
        $(document).on("click", ".user_list_clear", function () {
            var userId = $(this).attr('data-id');
            $.ajax({
                'data': { 'taskId': taskId,
                    'senderId': -1,
                    'receiverId': -1
                },
                'dataType': 'JSON',
                'type': 'POST',
                'url': '/HumanTasks/MoveTask',
                success: function () {
                    location.reload(true);
                }
            });
            $(".user_list_clear").off();
        });
    });

    /**** Tooltip  ******/
    $('.dashboard_btn').tooltip({
        position: 'left',
        delay: { show: 1000 }
    });
    /***** Filter  *******/
    $("#left_panel>input").val("Filter tasks");

    $("#left_panel>input").on("blur", function () {
        if ($(this).val() == "")
            $(this).val("Filter tasks");
    });

    $("#left_panel>input").on("click", function () {
        if ($(this).val() == "Filter tasks")
            $(this).val("");
    });


    /******** Project name length *******/

    $(".project_name").each(function () {
        var text = $(this).html();
        if (text.length > 20) {
            $(this).attr("title", text);
            $(this).html(text.substr(0, 20) + "...");
        }
    });
    if ($(".project_name").length == 0)
        $(".project_list").html("<span>There are no projects yet</span>");
    /********* Delete project ********/

    $("#delete_box").on('hidden', function () {
        $(".yes").off();
        $(window).off("keydown");
    });

    $(document).on("click", ".delete_btn", function () {
        var proj = $(this).parent(".proj_row").attr("data-id");
        $(".modal-body").html("<p>Are you sure, that you want to delete project</p>\
            <span> <b>" + $(this).parent(".proj_row").children(".project_name").html() + "</b>?</span>");
        $("#delete_box").modal('show');
        $(".yes").on("click", function () {
            deleteProjectQuery(proj);
            $(".yes").off();
        });
        $(window).on("keydown", function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                deleteProjectQuery(proj);
                $("#delete_box").modal('hide');
            }
            $(window).off("keydown");
        });
    });

    /******** Add Project *********/
    $(".new_project_btn").click(function () {
        if ($(".newBox").length == 0) {
            $(".project_list").prepend($("#add_proj_box").clone());
            $("#add_proj_box").addClass("newBox");
            $(".newBox").css("display", "block");
        } else {
            $(".newBox").children("input").focus();
        }
        $(".add_cancel").click(function () {
            $(".newBox").remove();
        });
        $(".add_proj_btn").click(function () {
            sendNewProject();
        });
        $(".newBox").children("input").keyup(function (e) {
            var code = null;
            code = (e.keyCode ? e.keyCode : e.which);
            if (code == 27) {
                $(".newBox").remove();
            } else if (code == 13) {
                sendNewProject();
            }
        });
    })
        ;

    /********* Project Tasks View  *******/
    var homeProj = $("#projects .proj_row:first-child").attr("data-id");
    $("#projects .proj_row:first-child").children(".project_name").addClass("active_proj");
    getTaskList(homeProj);
    ko.applyBindings(modelData);
    $('body').popover({
        selector: '.assignee_popover,.task_popover',
        delay: { show: 100 },
        placement: 'top'
    });

    $(document).on("click", ".task_group_list .project_name", function () {
        $(".project_name").removeClass("active_proj");
        $(this).addClass("active_proj");
        var url = null;
        switch ($(this).attr("id")) {
            case "all_tasks": url = "all"; break;
            case "my_tasks": url = "my"; break;
            case "unassigned_tasks": url = "unassigned"; break;
        }
        var groupName = $(this).html();
        getTaskGroup(url, groupName);
    });
    $(document).on("click", ".user_projs,.created_projs", function () {
        $(".project_name").removeClass("active_proj");
        $(this).addClass("active_proj");
        var proj = $(this).parent(".proj_row").attr("data-id");
        getTaskList(proj);
    });

    $('#ProjectHumanTask').change(function () {
        for (var i = 0; i < modelData.tasks().length; i++) {
            modelData.tasks()[i]._destroy = false;
        }
        modelData.tasks.destroy(function (item) {
            return item.Name.indexOf($("#ProjectHumanTask").val()) == -1;
        });
        var undestroyedExists = false;
        for (var i = 0; i < modelData.tasks().length; i++) {
            if (modelData.tasks()[i]._destroy == false) {
                undestroyedExists = true;
                break;
            }
        }
        if (undestroyedExists == false) {
            for (var i = 0; i < modelData.tasks().length; i++) {
                modelData.tasks()[i]._destroy = false;
            }
        }
    });
});
function getTaskList(proj) {
    $.ajax({
        data: { projectId: proj },
        dataType: "JSON",
        type: "GET",
        url: "/Landing/GetTasks",
        beforeSend: function () {
            //$("#loader").show();
        },
        success: function (data) {
            var task = null;
            modelData.project(data.Name);
            $("#content").children(".proj_title").html("[project]");
            modelData.tasks.removeAll();
            modelData.users.removeAll();
            for (var i = 0; i < data.Tasks.length; i++) {
                task = data.Tasks[i];
                var date = new Date(parseInt(task.Created.substr(6)));
                var thisTask = new Object({
                    Id: task.Id,
                    Name: task.Name,
                    Description: task.Description,
                    CreatedDate: date.toLocaleDateString(),
                    CreatedTime: date.toLocaleTimeString(),
                    Creator: task.Creator,
                    Priority: task.Priority,
                    AssigneeId: task.AssigneeId,
                    Assignee: task.Assignee,
                    AssigneePhoto: task.AssigneePhoto
                });
                modelData.tasks.push(thisTask);
            }
            for (var i = 0; i < data.Users.length; i++) {
                var user = data.Users[i];
                var thisUser = new Object({
                    Id: user.Id,
                    Name: user.Name,
                    Photo : user.Photo==true? '/Project/GetImage?UserId='+user.Id : '/Content/images/photo.png'
                });
                modelData.users.push(thisUser);
            }
        }
    });
}

function getTaskGroup(url, groupName) {
    $.ajax({
        data: { 'projectId'   : "-1",
                'taskGroup'   : url},
        dataType: "JSON",
        type: "GET",
        url: "/Landing/GetTasks",
        success: function (data) {
            var task = null;
            modelData.project(data.Name);
            $("#content").children(".proj_title").html("[" + groupName + "]");
            modelData.tasks.removeAll();
            modelData.users.removeAll();
            for (var i = 0; i < data.Tasks.length; i++) {
                task = data.Tasks[i];
                var date = new Date(parseInt(task.Created.substr(6)));
                var thisTask = new Object({
                    Id: task.id,
                    Name: task.name,
                    Description: task.Description,
                    CreatedDate: date.toLocaleDateString(),
                    CreatedTime: date.toLocaleTimeString(),
                    Creator: task.Creator,
                    Priority: task.Priority,
                    AssigneeId: task.AssigneeId,
                    Assignee: task.Assignee,
                    AssigneePhoto: task.AssigneePhoto
                });
                modelData.tasks.push(thisTask);
            }
        }
    });
}

function sendNewProject() {
    var val = $(".newBox").children("input").val();
    $(".newBox").remove();
    $.ajax({
        data: { projectName: val },
        dataType: "JSON",
        type: "POST",
        url: "/Landing/AddProject",
        beforeSend: function () {
            $("#loader").show();
        },
        success: function (response) {
            $("#loader").hide();
            listProjects(response);
        }
    });
}


function deleteProjectQuery(proj) {
    $.ajax({
        data: { projectId: proj },
        dataType: 'JSON',
        type: 'POST',
        url: '/Landing/DeleteProject',
        beforeSend: function () {
            $("#loader").show();
        },
        success: function (response) {
            $("#loader").hide();
            listProjects(response);
        }
    });
}

function listProjects(response) {
    var list = $(".project_list");
    list.html("");
    var name = null;
    var creatorProjectsCount = projectsOutput(response.CreatorProjects, "created_projs");
    var userProjectsCount = projectsOutput(response.UserProjects, "user_projs");
    if (userProjectsCount + creatorProjectsCount == 0) {
        $(".project_list").html("<span>There are no projects yet</span>");
    }
    $('.project_name').each(function () {
        if ($(this).html() == $('#content h2').html()) {
            $(this).addClass('active_proj');
        }
    });
}

function projectsOutput(projects, li_class) {
    var name = null;
    for (var i = 0; i < projects.length; i++) {
        if (projects[i].Name.length < 20)
            name = projects[i].Name;
        else
            name = projects[i].Name.substr(0, 20) + '...';
        var ownProjDelete = (li_class == 'user_projs') ? "" : "<div class='delete_btn'></div>";
        $(".project_list").append("\
        <div class='proj_row' data-id='" + projects[i].Id + "'>\
                         " + ownProjDelete + "\
                         <div  class='" + li_class + " project_name'>" + name + "</div>\
                     </div>");
         if (projects[i].Name.length >= 20)
            $("li[data-id=" + projects[i].Id + "]").attr("title", projects[i].Name);
        $('.dashboard_btn').tooltip({
            'placement': 'right',
            delay: { show: 1000 }
        });
    }
    return i;
}