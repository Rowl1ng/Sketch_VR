# Sketch_VR

## Game Instruction

Run the game, then:

## Scene 1: 2D start menu

- input: 
    - player name
    - directory for saving sketchs
    - path of namelist file (txt format) : each line is a model name

![Capture.PNG-40.4kB][1]

## Scene 2: sketch in VR

![scene.PNG-1322.1kB][6]

Functions on hand controllers:

![6AC605AD-AECF-49A4-84C2-EBA5F9EE05FC_1_201_a.jpeg-65.8kB][2]

- to **rotate** and **drag** the model: keep pressing the **hand trigger** of right hand and rotate your right controller, the model and sketch will follow it.
- to **draw** lines: keep pressing the **front trigger** of right hand controller. When you release the trigger the line also ends.
- to **undo** the last stroke: press `X` button on the left hand controller
- to hide the model when you want to check your sketch or start setching by recollection: press `Y` button on the left hand controller

Functions on UI: use laserpoint on right hand and press `A` button on right controller to click any button on UI.

![ui.PNG-625.6kB][3]

- to clear all sketch: click the `Clear Sketch` button
- to start sketching without reference: click the `Memorize Mode` button
- to save the sketch: click the `Save Sketch` button
- move on to next model: click the `Next Model` button
- to quit the game: click the `Quit` button

> Optional: You can choose any color you like on the `ColorPicker` of your left controller just by dragging the black rectangle on the ColorPicker plane. How to drag? Use the laser point to hit the black rectangle and keep pressing the **hand trigger** of right hand controller.

![colorpicker.PNG-352kB][4]

![Jan-14-2020 11-23-10.gif-8607.3kB][5]

### Task 1: sketch with reference



![Jan-14-2020 11-31-19.gif-11879.8kB][7]

Try to draw the contour of the model and click `Save Sketch` when you finish the sketch.

### Task 2: sketch without reference


Press "`memorize mode`" and start countdown (30 seconds, you can press Y button to hide the model and start sketching at any time when you memorizes the model). Player has to memorize the model in limited time. When the time runs out, the model will vanish:

![Jan-14-2020 11-36-14.gif-11529.5kB][8]

Now player can start sketching **the same model** by recollecting from memory instead of with a reference:

![Jan-14-2020 11-44-10.gif-9443.8kB][9]


If you finish both tasks for the current model, you can click on `Next Model` button to move on to next model:

![Jan-14-2020 11-33-13.gif-6677.3kB][10]

## Develoment

More information about development can be seen on my blog: https://rowl1ng.com/blog/tech/Sketch-VR.html

  [1]: http://static.zybuluo.com/sixijinling/hgbw8jf4ur1x4k9mlgpu389j/Capture.PNG
  [2]: http://static.zybuluo.com/sixijinling/vjyac7c1uonbvquuscpea1xl/6AC605AD-AECF-49A4-84C2-EBA5F9EE05FC_1_201_a.jpeg
  [3]: http://static.zybuluo.com/sixijinling/blcx91n07lmf803xzey6kksv/ui.PNG
  [4]: http://static.zybuluo.com/sixijinling/ndghatp2hshzuvinrvq96c9a/colorpicker.PNG
  [5]: http://static.zybuluo.com/sixijinling/yxnefnprfgh977s71fkdhinn/Jan-14-2020%2011-23-10.gif
  [6]: http://static.zybuluo.com/sixijinling/az5zki5jecwi1srrchai0939/scene.PNG
  [7]: http://static.zybuluo.com/sixijinling/1cvw2b4zppmzyzjbgp9czgjd/Jan-14-2020%2011-31-19.gif
  [8]: http://static.zybuluo.com/sixijinling/khs6qimbgce71skdnoqohkqr/Jan-14-2020%2011-36-14.gif
  [9]: http://static.zybuluo.com/sixijinling/06wzfzvpemw8s8waz3zp6n01/Jan-14-2020%2011-44-10.gif
  [10]: http://static.zybuluo.com/sixijinling/1rkw634yzkw08pgrht0raqqi/Jan-14-2020%2011-33-13.gif