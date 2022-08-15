// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
  $("#close-row").html("<div></div></div>");
  $(".wrap-5").wrap("<div class='col-md-5'></div></div>");
  $(".s2createcat").select2();
  $(".sTag").select2();

  $(".s2createtag").select2();
  $(".s2cat").select2();
  tinyMCE.init({
    selector: ".create-post ",
  });

  function readURL(input) {
    if (input.files && input.files[0]) {
      var reader = new FileReader();

      reader.onload = function (e) {
        $("#editimg").attr("src", e.target.result);
      };

      reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
  }

  $("#myimg").change(function () {
    readURL(this);
  });

  $(".dropdown").hover(function () {
    var dropdownMenu = $(this).children(".dropdown-menu");
    if (dropdownMenu.is(":visible")) {
      dropdownMenu.parent().toggleClass("open");
    }
  });

  var cat = $(".cat");
  var post = $(".pt");
  var tag = $(".tg");
  // if (cat || post || tag) {
  //     post.DataTable();
  //     cat.DataTable();
  //     tag.DataTable();
  // }
  if (cat || tag) {
    post.DataTable();
    cat.DataTable();
    tag.DataTable();
  }

  // $("#ptable").DataTable({
  //   processing: true,
  //   serverSide: true,
  //   serverMethod: "get",
  //   ajax: {
  //     url: "https://localhost:5001/api/posts/alnews",
  //   },
  //   columns: [{ data: "name" }, { data: "created_at" }],
  // });

  var sty = document.getElementById("progress-bar");
  if (sty) {
    let processScroll = () => {
      let docElem = document.documentElement,
        docBody = document.body,
        scrollTop = docElem["scrollTop"] || docBody["scrollTop"],
        scrollBottom =
          (docElem["scrollHeight"] || docBody["scrollHeight"]) -
          window.innerHeight,
        scrollPercent = (scrollTop / scrollBottom) * 100 + "%";

      // console.log(scrollTop + ' / ' + scrollBottom + ' / ' + scrollPercent);

      sty.style.setProperty("--scrollAmount", scrollPercent);
    };

    document.addEventListener("scroll", processScroll);
  }

  $.ajax({
    url: "/api/comments/",
    type: "GET",
    success: function (res) {
      $(".alert-num").text(res);
    },
  });

  $.ajax({
    url: "/api/posts/draft/count",
    type: "GET",
    success: function (res) {
      $(".draft-num").text(res);
    },
  });

  $.ajax({
    url: "/api/posts/trash/count",
    type: "GET",
    success: function (res) {
      $(".trash").text(res);
    },
  });

  //let uniquehits = JSON.stringify({ uniqueHits: 1 });
  //$.ajax({
  //    url: "/api/stats",
  //    type: 'POST',
  //    data: uniquehits,

  //    contentType: "application/json",
  //    success: function (res) {
  //        alert("don");
  //    }
  //});

  $.ajax({
    url: "/api/categories",
    type: "GET",
    success: function (data) {
      for (var i = 0; i < data.length; ++i) {
        i < 6
          ? $("#main-nav").append(
              `
                <li class="nav-item">
                    <a class="nav-link text-dark" href="/posts/category/${data[i].slug}">
                ${data[i].name}

                    </a>

                </li>
        `
            )
          : "";
      }
    },
  });

  //$(".dbtn").click(function (event) {
  //    var btn = $(".dbtn");
  //    var id = Number($('.dbtn').attr("data-id"));
  //    let data = JSON.stringify({ id: id });

  //    alertify.confirm("Are you sure to Delete?",
  //        function () {
  //            $.ajax({
  //                method: "DELETE",
  //                url: "/api/posts/" + id,
  //                data: data,
  //                contentType: "application/json",
  //            }).done(function () {
  //                event.target.parentElement.parentElement.remove();

  //                alertify.success('Post is Deleted!');
  //            }).fail(function (msg) {
  //                console.log('FAIL');
  //            }
  //            ).always(function (msg) {

  //            });
  //        },
  //        function () {
  //            alertify.error('Cancelled');
  //        });
  //})
});

$(document).ready(function () {
  function getPostsForAdmin(url) {
    $.ajax({
      url: url,
      type: "GET",
      beforeSend: function () {
        $("#load").show();
      },
      success: function (res) {
        let totalPages = res.totalPages;
        let pageSize = res.pageSize;
        let pageNumber = res.pageNumber;
        let totalRecords = res.totalRecords;

        function getNumber() {
          let li = "";
          let droplist = $("#showRows");
          for (i = 1; i < totalPages; ++i) {
            if (i <= 10) {
              li += `<li><a class='btn-call ${
                i == pageNumber ? "active" : ""
              }' data-id='api/posts/alnews?pageNumber=${i}&pageSize=5'>${i}</a></li>`;
            }
            if (i > 10) {
              droplist.append(
                `<option class='btn-call' value='api/posts/alnews?pageNumber=${i}&pageSize=5'>${i}</option>`
              );
            }
          }
          return li;
        }

        var pg = $(".pgn");
        pg.html(`
      <div class="d-flex align-items-center mx-3"> Total records ${totalRecords} | Page ${pageNumber} of ${totalPages} </div>
            <li><a class="btn-call ${
              pageNumber === 1 ? "disabled" : ""
            }" data-id="${res.previousPage}">&#8592;</a></li>
       ${getNumber()}
                            <li><a  class="btn-call" data-id="${
                              res.nextPage
                            }">&#8594;</a></li>
            `);

        $.each(res.data, function (key, val) {
          var tbl = $("#ptablbody");

          let x = tbl.append(`
                <tr style='height:85px'>
                  <td class='action-table'>
                    ${val.name}
                       <span class='action-link'>
                 <span class="text-danger dbtn" data-id="${
                   val.id
                 }"> Delete </span>|
                                            <a href="/posts/edit/${
                                              val.id
                                            }">Edit</a> |
                                            <a href="/posts/news/${
                                              val.slug
                                            }">View</a>
                        </span>
                  </td>
                  <td>${val.postCategories
                    .map((cat) => cat.category.name)
                    .join(",")}</td>
                  <td>${val.postTags.map((cat) => cat.tag.name).join(",")}</td>
                   <td>
                   ${new Date(val.createdDate)}
                  </td>
                  <td class='d-flex'><img width="25px" height="25px" class="rounded-circle mx-2" src="/uploads/${
                    val.picture !== null ? val.profileImg : "default.jpg"
                  }"><div> ${val.userName.substring(
            0,
            val.userName.indexOf("@")
          )}</div></td>
                </tr>
                  
                
                `);
        });
      },
      complete: function () {
        $("#load").hide();
      },
    });
  }

  $("#searchBtn").click(function () {
    let vl = $("#search").val();
    getPostsForAdmin(`api/posts/alnews/?pageNumber=1&pageSize=10?search=${vl}`);
  });
  getPostsForAdmin("api/posts/alnews");
  $("#showRows").change(function (e) {
    let num = $("#showRows").val();
    getPostsForAdmin(num);
    $("#ptablbody").empty();
  });

  // "#showRows".val("showRows").change();
  $(document).on("click", "span.dbtn", function () {
    // $(this).parent().remove();

    delPost(this);
  });

  $(document).on("click", "a.btn-call", function () {
    getPostsForAdmin($(this).attr("data-id"));
    $("#ptablbody").empty();
  });

  function delPost(event) {
    var id = Number($(event).attr("data-id"));
    let data = JSON.stringify({
      postStatus: 0,
    });

    alertify.confirm(
      "Are you sure to Delete?",
      function () {
        $.ajax({
          method: "POST",
          url: "/api/posts/" + id,
          data: data,
          contentType: "application/json",
        })
          .done(function () {
            $(this).parent().parent().parent().remove();
            $("#ptablbody").empty();
            getPostsForAdmin("api/posts/alnews");
            alertify.success("Post is Deleted!");
          })
          .fail(function (msg) {
            console.log("FAIL");
          })
          .always(function (msg) {});
      },
      function () {
        alertify.error("Cancelled");
      }
    );
  }
});
