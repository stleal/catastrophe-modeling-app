$(document).ready(function () {

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
      },
      error: function (jqXHR, textStatus, errorThrown) {
        console.error("File upload failed:", textStatus, errorThrown);
        console.error("Server response:", jqXHR.responseText);
      }
    });
  });
});