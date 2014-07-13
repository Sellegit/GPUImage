
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoTouch.Dialog;

using GPUImage.Filters;
using GPUImage;

namespace GPUImage.Sample
{
	/// <summary>
	/// Display an image and apply any selected filter
	/// </summary>
	public partial class MainViewController : UIViewController
	{
		private UIImage image;
		private GPUImageView imageView;

		public MainViewController () : base (null, null)
		{
    }
//
//		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
//		{
//			return UIInterfaceOrientationMask.Landscape;
//		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		// Create view
    public override void ViewWillAppear (bool animated)
    {
      base.ViewWillAppear (animated);
      UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, true);

    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();




			
      image = UIImage.FromFile("l92l.png");

			imageView = new GPUImageView(new RectangleF(0,0,480,320));
			imageView.ContentMode = UIViewContentMode.ScaleToFill;
			View = imageView;

			// Trick to display the image initially
			// TODO: Have the right size

      var radius = 5.5f;
      var maxRadius = 20.0f;
      var saturation = 1f;
      var maxSaturation = 2.0f;
      var downsampling = 4.0f;
      var maxDownsampling = 30.0f;
      var rangeReductionFactor = 0.6f;
      var maxRangeReductionFactor = 1.0f;


      var luminance = 0.6f;
      var maxLuminance = 1.0f;

      applyBlurFilter (radius, saturation, downsampling, rangeReductionFactor, luminance);

      {
        var radiusLabel = new UILabel (new RectangleF (0, 10, 320, 20)) { Text = "Radius: " + radius, TextColor = UIColor.White };
        UISlider radiusSlider = new UISlider (new RectangleF (0, 30, 320, 40));
        radiusSlider.Value = radius / maxRadius;
        radiusSlider.ValueChanged += (object sender, EventArgs e) => {
          radius = ((int)(maxRadius * radiusSlider.Value * 10)) / 10f;
          radiusLabel.Text = "Radius: " + radius;
        };
        radiusSlider.AddTarget (delegate (object sender, EventArgs e) {
          radius = ((int)(maxRadius * radiusSlider.Value * 10)) / 10f;
          applyBlurFilter (radius, saturation, downsampling, rangeReductionFactor, luminance);
          radiusLabel.Text = "Radius: " + radius;
        }, UIControlEvent.TouchUpInside);
        View.AddSubview (radiusLabel);
        View.AddSubview (radiusSlider);
      }

      {
        var saturationLabel = new UILabel (new RectangleF (0, 130, 320, 20)) { Text = "Saturation: " + saturation, TextColor = UIColor.White };
        UISlider saturationSlider = new UISlider (new RectangleF (0, 150, 320, 40));
        saturationSlider.Value = saturation / maxSaturation;
        saturationSlider.ValueChanged += (object sender, EventArgs e) => {
          saturation = ((int)(maxSaturation * saturationSlider.Value * 10)) / 10f;
          saturationLabel.Text = "Saturation: " + saturation;
        };
        saturationSlider.AddTarget (delegate (object sender, EventArgs e) {
          saturation = ((int)(maxSaturation * saturationSlider.Value * 10)) / 10f;
          applyBlurFilter (radius, saturation, downsampling, rangeReductionFactor, luminance);
          saturationLabel.Text = "Saturation: " + saturation;
        }, UIControlEvent.TouchUpInside);
        View.AddSubview (saturationLabel);
        View.AddSubview (saturationSlider);
      }

      {
        var luminanceLabel = new UILabel (new RectangleF (0, 70, 320, 20)) { Text = "Luminance: " + luminance, TextColor = UIColor.White };
        UISlider luminanceSlider = new UISlider (new RectangleF (0, 90, 320, 40));
        luminanceSlider.Value = luminance / maxLuminance;
        luminanceSlider.ValueChanged += (object sender, EventArgs e) => {
          saturation = ((int)(maxLuminance * luminanceSlider.Value * 10)) / 10f;
          luminanceLabel.Text = "Luminance: " + luminance;
        };
        luminanceSlider.AddTarget (delegate (object sender, EventArgs e) {
          saturation = ((int)(maxLuminance * luminanceSlider.Value * 10)) / 10f;
          applyBlurFilter (radius, saturation, downsampling, rangeReductionFactor, luminance);
          luminanceLabel.Text = "Luminance: " + luminance;
        }, UIControlEvent.TouchUpInside);
        View.AddSubview (luminanceLabel);
        View.AddSubview (luminanceSlider);
      }

		}

    private void applyBlurFilter(float radius, float saturation, float downsampling, float rangeReductionFactor, float luminanceRangeReductionFactor) {

      var filter = new GPUImageiOSBlurFilter ();
      filter.BlurRadiusInPixels = radius;
      filter.Saturation = saturation;
      filter.Downsampling = downsampling;
      filter.RangeReductionFactor = rangeReductionFactor;

      var filter2 = new GPUImageLuminanceRangeFilter ();
      filter2.RangeReductionFactor = luminanceRangeReductionFactor;
//
//			// Apply a filter
//			GPUImageSepiaFilter filter = new GPUImageSepiaFilter();
//			filter.Intensity = sepiaIntensity;
//			
			// Create image object to process
			GPUImagePicture stillImageSource = new GPUImagePicture(image, true);
			stillImageSource.AddTarget(filter);
      stillImageSource.AddTarget (filter2);
      //stillImageSource.add
			
			filter.AddTarget(imageView);

			// Launch the process
			stillImageSource.ProcessImage();
		}
	}
}

