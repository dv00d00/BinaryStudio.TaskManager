var modelData = new TaskModel();
$(function () {

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
    getTaskList(homeProj);
    ko.applyBindings(modelData);
    
    $('body').popover({
        selector: '.task_popover',
        delay: { show: 300 }
    });

    $(document).on("click", ".project_name", function () {
        $(".project_name").removeClass("active_proj");
        $(this).addClass("active_proj");
        var proj = $(this).parent(".proj_row").attr("data-id");
        getTaskList(proj);
    });

});
function getTaskList(proj) {
    $.ajax({
        data: { projectId: proj },
        dataType: "JSON",
        type: "POST",
        url: "/Landing/GetTasks",
        beforeSend: function () {
            //$("#loader").show();
        },
        success: function (data) {
            var task = null;
            $("#content").children("h2").html(data.Project.Name);
            $("#content").children(".proj_title").html("[project]");
            modelData.tasks.removeAll();
            for (var i = 0; i < data.Project.Tasks.length; i++) {
                task = data.Project.Tasks[i];
                var date = new Date(parseInt(task.Created.substr(6)));
                dateFormat(date, "longTime");
                var thisTask = new Task(task.Id, task.Name, task.Description, date, task.Creator);
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
                         <a href='/Project/Project/" + projects[i].Id + "'><div class='dashboard_btn' title='" + projects[i].Name + " dashboard'></div></a>\
                     </div>");
         if (projects[i].Name.length >= 20)
            $("li[data-id=" + projects[i].Id + "]").attr("title", projects[i].Name);
        $('.dashboard_btn').tooltip({
            position: 'left',
            delay: { show: 1000 }
        });
    }
    return i;
}