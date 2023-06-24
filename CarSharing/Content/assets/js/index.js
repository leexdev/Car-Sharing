window.addEventListener('load', function() {
    var slider = document.getElementById('slider');
    var images = ['/Content/assets/img/pexels-craig-adderley-1563356.jpg', '/Content/assets/img/pexels-nout-gons-248159.jpg', '/Content/assets/img/pexels-tiana-614484.jpg'];
  
    // Hàm thay đổi background slider
    function changeBackground() {
      var randomIndex = Math.floor(Math.random() * images.length);
      var randomImage = images[randomIndex];
      slider.style.backgroundImage = 'url("' + randomImage + '")';
    }
  
    // Thay đổi background ban đầu
    changeBackground();
  
    // Sự kiện beforeunload khi rời khỏi trang
    window.addEventListener('beforeunload', function() {
      changeBackground();
    });
  });
  

  var currentDateStart = new Date();
  var currentDateEnd = new Date();
  currentDateStart.setUTCHours(7, 0);
  currentDateEnd.setUTCHours(19, 0);

  var currentDateTimeStart = currentDateStart.toISOString().slice(0, 16);
  var currentDateTimeEnd = currentDateEnd.toISOString().slice(0, 16);

  // Thiết lập giá trị mặc định cho ngày nhận và ngày trả xe trên trang "index.html"
  document.getElementById("dateTimePickerStart").value = currentDateTimeStart;
  document.getElementById("dateTimePickerEnd").value = currentDateTimeEnd;
  localStorage.setItem("startDateStorage", currentDateTimeStart);
  localStorage.setItem("endDateStorage", currentDateTimeEnd);