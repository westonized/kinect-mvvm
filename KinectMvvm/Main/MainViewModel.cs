﻿using KinectMvvm.Framework;
using KinectMvvm.Head;
using KinectMvvm.Sensor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace KinectMvvm.Main
{
    public class MainViewModel : BaseViewModel
    {
        ISensor sensor;

        public MainViewModel(ISensor sensor)
        {
            this.sensor = sensor;
            this.sensor.NewColourFrame += UpdateColourOutput;
            this.sensor.NewHeadFrame += UpdateHeadOutput;
            this.sensor.NewInfraredFrame += UpdateInfraredOutput;
        }

        private void UpdateInfraredOutput(object sender, FrameEventArgs e)
        {
            InfraredData = (byte[])e.Frame.Data;
        }

        private void UpdateHeadOutput(object sender, FrameEventArgs e)
        {
            List<Tuple<double, double, double>> heads = (List<Tuple<double, double, double>>)e.Frame.Data;

            if (heads.Count > 0)
            {
                HeadViewModel.Update(heads[0]);
            }
        }

        private void UpdateColourOutput(object sender, FrameEventArgs e)
        {
            ColourData = (byte[]) e.Frame.Data;
        }

        private HeadViewModel headViewModel;
        public HeadViewModel HeadViewModel
        {
            get
            {
                if (this.headViewModel == null)
                {
                    this.headViewModel = new HeadViewModel();
                }

                return this.headViewModel;
            }
        }

        byte[] infraredData;
        public byte[] InfraredData
        {
            get
            {
                return this.infraredData;
            }

            set
            {
                this.infraredData = value;
                RaisePropertyChanged("InfraredData");
            }
        }

        byte[] colourData;
        public byte[] ColourData
        {
            get
            {
                return this.colourData;
            }

            set
            {
                this.colourData = value;
                RaisePropertyChanged("ColourData");
            }
        }

        public string Greeting
        {
            get
            {
                return "Kinect v2 and XAML!";
            }
        }
    }
}