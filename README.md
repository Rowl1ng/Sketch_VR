# Sketch VR

Sketch VR is designed to collect 3D sketch data from players. It has three main functions:

1. Load 3D models from given directory and display it in game on runtime. 
2. Sketch in VR.
3. Save sketch, combination of sketch and model.

![Jan-23-2020 13-30-49.gif-3171.3kB][1]

It is not only a VR painting game like Quill, it can also be used for viewing 3D models (For example, VR online shopping for furnitures!). If you are interested, it can be further developed into a 3D prototyping tool after adding editing function.

![Jan-23-2020 13-35-06.gif-1365.9kB][2]

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

- [Unity VR Tutorial: How To Build Tilt Brush From Scratch][3]
- [Building a Simple Color Picker from Scratch][4]
- [3D-VR-Painting][5]


  [1]: http://static.zybuluo.com/sixijinling/1jxlgdq7c26tudqyqiyn83nm/Jan-23-2020%2013-30-49.gif
  [2]: http://static.zybuluo.com/sixijinling/e5fsxqs3fbj32qg8k8lrsx58/Jan-23-2020%2013-35-06.gif
  [3]: https://www.youtube.com/watch?v=eMJATZI0A7c
  [4]: https://www.youtube.com/watch?v=wysIsMEQ3_Y
  [5]: https://github.com/E-BAO/3D-VR-Painting