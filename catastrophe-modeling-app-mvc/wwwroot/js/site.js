$(document).ready(function () {
  var stormState = {
    storms: [],
    pageSize: 10,
    currentPage: 1
  };

  function getValue(data, camelCaseName, pascalCaseName) {
    return data[camelCaseName] ?? data[pascalCaseName] ?? "";
  }

  function escapeHtml(value) {
    return $("<div></div>").text(value ?? "").html();
  }

  function normalizeStorms(data) {
    if (Array.isArray(data)) {
      return data;
    }

    if (!data || typeof data !== "object") {
      return [];
    }

    var collection = data.storms ?? data.Storms ?? data.items ?? data.Items ?? data.data ?? data.Data;
    if (Array.isArray(collection)) {
      return collection;
    }

    return [data];
  }

  function formatDate(value) {
    if (!value) {
      return "";
    }

    var parsedDate = new Date(value);
    if (Number.isNaN(parsedDate.getTime())) {
      return value;
    }

    return parsedDate.toLocaleDateString();
  }

  function buildPagination(totalPages) {
    var pagination = $("#stormPagination");
    pagination.empty();

    if (totalPages <= 1) {
      return;
    }

    var previousButton = $("<button type=\"button\" class=\"page-button\">Previous</button>");
    previousButton.prop("disabled", stormState.currentPage === 1);
    previousButton.on("click", function () {
      if (stormState.currentPage > 1) {
        stormState.currentPage -= 1;
        renderStormTable();
      }
    });
    pagination.append(previousButton);

    for (var pageNumber = 1; pageNumber <= totalPages; pageNumber += 1) {
      var pageButton = $("<button type=\"button\" class=\"page-button\"></button>");
      pageButton.text(pageNumber);
      pageButton.toggleClass("active", pageNumber === stormState.currentPage);
      pageButton.on("click", { pageNumber: pageNumber }, function (event) {
        stormState.currentPage = event.data.pageNumber;
        renderStormTable();
      });
      pagination.append(pageButton);
    }

    var nextButton = $("<button type=\"button\" class=\"page-button\">Next</button>");
    nextButton.prop("disabled", stormState.currentPage === totalPages);
    nextButton.on("click", function () {
      if (stormState.currentPage < totalPages) {
        stormState.currentPage += 1;
        renderStormTable();
      }
    });
    pagination.append(nextButton);
  }

  function renderStormTable() {
    var tableBody = $("#stormInfoTableBody");
    tableBody.empty();

    var totalStorms = stormState.storms.length;
    var totalPages = totalStorms === 0 ? 0 : Math.ceil(totalStorms / stormState.pageSize);

    if (totalPages > 0 && stormState.currentPage > totalPages) {
      stormState.currentPage = totalPages;
    }

    var startIndex = (stormState.currentPage - 1) * stormState.pageSize;
    var pageStorms = stormState.storms.slice(startIndex, startIndex + stormState.pageSize);

    $.each(pageStorms, function (_, storm) {
      tableBody.append([
        "<tr>",
        "<td>", escapeHtml(getValue(storm, "stormName", "StormName")), "</td>",
        "<td>", escapeHtml(formatDate(getValue(storm, "dateOfLandfall", "DateOfLandfall"))), "</td>",
        "<td>", escapeHtml(getValue(storm, "maxWind", "MaxWind")), "</td>",
        "</tr>"
      ].join(""));
    });

    $("#stormInfoSection").toggle(totalStorms > 0);
    buildPagination(totalPages);
  }

  function setStorms(storms) {
    stormState.storms = storms;
    stormState.currentPage = 1;
    renderStormTable();
  }

  function loadInitialStorms() {
    var initialStormDataElement = document.getElementById("initialStormData");
    if (!initialStormDataElement) {
      return [];
    }

    var json = initialStormDataElement.textContent;
    if (!json) {
      return [];
    }

    try {
      return normalizeStorms(JSON.parse(json));
    } catch (error) {
      console.error("Unable to parse initial storm data.", error);
      return [];
    }
  }

  $("#pageSizeSelect").on("change", function () {
    stormState.pageSize = Number($(this).val()) || 10;
    stormState.currentPage = 1;
    renderStormTable();
  });

  setStorms(loadInitialStorms());

  $("#submitBtn").on("click", function (event) {
    event.preventDefault();

    var input = document.getElementById("fileUpload");
    var file = input.files[0];
    if (!file) {
      console.error("No file selected.");
      return;
    }

    var formData = new FormData();
    formData.append("file", file);

    $.ajax({
      url: "https://localhost:7075/api/Process/ProcessHURDAT2",
      type: "POST",
      data: formData,
      processData: false,
      contentType: false,
      success: function (data) {
        console.log("File uploaded successfully:", data);
        var storms = normalizeStorms(data);
        setStorms(storms);
      },
      error: function (jqXHR, textStatus, errorThrown) {
        console.error("File upload failed:", textStatus, errorThrown);
        console.error("Server response:", jqXHR.responseText);
      }
    });
  });
});