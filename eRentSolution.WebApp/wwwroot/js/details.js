       
var boxSlider = document.querySelectorAll(".box-photo-slider");
boxSlider[0].classList.add("hienthi")
                var slideIndex = 1;
        showSlides(slideIndex);

        // Next/previous controls
        function plusSlides(n) {
        showSlides(slideIndex += n);
        }

        // Thumbnail image controls
        function currentSlide(n) {
        showSlides(slideIndex = n);
        }

        function showSlides(n) {
            var i;
      
            var slides = document.querySelectorAll(".hienthi > .slideshow-container > .mySlides");
            
   
      
        var dots = document.getElementsByClassName("dot");
        if (n > slides.length) {slideIndex = 1}
        if (n < 1) {slideIndex = slides.length}
        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }
        for (i = 0; i < dots.length; i++) {
            dots[i].className = dots[i].className.replace(" active", "");
        }
        slides[slideIndex-1].style.display = "block";
        dots[slideIndex-1].className += " active";
        }


var options = document.querySelectorAll("#box-select option")

         function myFunction(){
             var value = document.querySelector("#box-select").value;
             console.log(value)
                for(let i =0; i<boxSlider.length;i++){
                    if(boxSlider[i].className.includes(value)){
                        document.querySelector(".box-photo-slider.hienthi").classList.remove('hienthi');
                        boxSlider[i].classList.add('hienthi');
                        var slideIndex = 0;
                        showSlides(slideIndex);
                    
                    }
                }
            
}
function showPhoneFunction() {
    document.querySelector("#box-showphone").style.display = "none";
    document.querySelector("#box-sdt").style.display = "block";
}