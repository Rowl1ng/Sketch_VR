# Sketch VR

Sketch VR is designed to collect 3D sketch data from players. It has three main functions:

1. Load 3D models from given directory and display it in game on runtime. 
2. Sketch in VR.
3. Save sketch, combination of sketch and model.

It is not only a VR painting game like Quill, it can also be used for viewing 3D models (For example, VR online shopping for furnitures!). If you are interested, it can be further developed into a 3D prototyping tool after adding editing function.

## Requirements

To run this project you need:

- Oculus Rift: headset + handset x 2 + sensor x 2 ;If you want to transfer to other VR platforms, you might need replace the input API
- System： Windows 10
- Development Software：Unity 2019.2.17f1 + Visual Studio 2019
- Language: C# 

## Usage

After cloning this project to your local path, you can open this project in Unity and load the `start` scene.

You need to create a directory for saving your sketches before starting sketching.

You can also start from vrpaint scene, where you need to replace the static variables in `PlayerManager.cs` before start programming:

```
public static string model_dir = @"..\demo_dataset";
public static string save_dir = @"..\demo_savedir";
public static string namelist_path = @"..\demo_namelist.txt";
```

## Contributing

More information about development is on my blog: https://rowl1ng.com/blog/tech/Sketch-VR.html

## Acknowledgement

- [Unity VR Tutorial: How To Build Tilt Brush From Scratch][1]
- [Building a Simple Color Picker from Scratch][2]
- [3D-VR-Painting][3]


  [1]: https://www.youtube.com/watch?v=eMJATZI0A7c
  [2]: https://www.youtube.com/watch?v=wysIsMEQ3_Y
  [3]: https://github.com/E-BAO/3D-VR-Painting