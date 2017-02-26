# Google Daydream Unity - Boxes

Here is a beginners guide to setting up a basic Daydream app where you can pick up boxes. You can clone the repo and getting working on Daydream right away, or follow the guide to see how the project was made.

## Prerequisites

A basic understanding of Unity and its interface.

* Unity Daydream Preview (I used 5.4.2f2-GVR13)
  * https://unity3d.com/partners/google/daydream (download link near bottom of page)
* JDK (I used 1.8, I think you can use 1.7)
  * http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html  
* Android SDK with v25 build tools
  * https://developer.android.com/studio/index.html#downloads (Look for section 'Get just the command line tools')
* Latest version of Google Daydream SDK for Unity (v1.20 at time of writing)
  * https://developers.google.com/vr/unity/download#google-vr-sdk-for-unity

## Setting up Unity for Android dev

To get started with the Daydream setup, we first need to set up Unity with the GoogleVR SDK and configure it to package the scene and deploy it to a Daydream ready device. To do this we also need to add the SDK paths for Java and android.

1. Start with a new scene in a new project
2. Find Unity preferences (Unity -> Preferences in Mac) and set the paths for:
  * Java SDK
  * Android SDK
3. In Unity menu click "Assets -> Import Package -> Custom Package" and navigate to the Google Daydream SDK for Unity.
  * Uncheck the iOS tree of imports
  * Uncheck demo scenes (makes build time longer?)
4. Set Unity platform to android (File -> Build Settings, select Android, click 'Switch Platform'). It's important to do this step now because the GoogleVR SDK scripts depend on this setting to work with Daydream, which is android specific.
5. In the Inspector for Player Settings (Edit -> Project Settings -> Player) navigate to the 'Other Settings' dropdown, turn on the 'Virtual Reality Supported' checkbox. Add Daydream if it is not already there and ensure it is at the top of the list.

Now that we have Unity ready to work with the GoogleVR SDK Daydream scripts, and build an android package, we can jump into the daydream boilerplate.

## Daydream Boxes Project

Daydream apps make use of a VR headset, which takes an android phone that displays a scene in a stereoscopic view, and a Daydream controller. Here we'll set up Unity to create that stereoscopic view, add controller support, and also set up an emulator on an android device to act as a controller within Unity so you can test your progress in the editor and not have to build to a device to view each change you make.

### Initial scene setup

1. Create a plane, which will act as the ground
2. Create a cube that will be set up as a box that you'll use to interact with with the Daydream controller
  * Place it above the ground plane
  * Add a rigid body component to it to enable gravity
3. Add materials to the box and ground plane and label them accordingly... unless you want to work with a boring scene.

#### Daydream specific setup

1. Create an empty game object and label it DaydreamPlayer
  * This will hold all our Daydream specific objects. At the end of this guide you should be able to make a prefab out of this object to begin using the Daydream controller in any project
2. Add the prefab GvrViewerMain as a child component of the DaydreamPlayer.
 * After adding this component to your scene, you should be able to run the scene and see it rendered in a stereoscopic view (split screen view). If you can't see the stereoscopic view, ensure that the build settings have been set to android (see 'Setting up Unity for Daydream dev' section of this guide for more details)
3. Add the prefab GvrEventSystem as a child component of the DaydreamPlayer. The GvrEventSystem is similar to Unity's default UI EventSystem with the following differences:
  * The "Standalone Input Module" script has been replaced with "Gvr Pointer Input Module" to allow the Daydream controller to be used as input
  * The "Gvr Pointer Manager" script has been added to control the various GoogleVR input devices. In our case it will automatically manage the Daydream controller.
4. Add the prefab GvrControllerMain as a child component of the DaydreamPlayer. This prefab contains the:
  * Arm model, which aims to give realistic movement of the controller
  * The GvrController script, which is the entry point to the controller API
5. Add the prefab GvrControllerPointer as a child component of the DaydreamPlayer. This prefab contains:
  * A model of the controller that will appear in our scene
  * The laser pointer, which also appear in our scene to indicate where the controller is pointing
  * The script GvrControllerVisualManager, which will hide/show the controller model if the controller is disconnected/connected
6. To set up the emulator controller so we can run our scene in the Unity Editor and control the pointer, follow the steps for `Daydream controller emulator setup`. Alternatively, you can build to a Daydream ready device and use a real controller with the steps for `Building to android device`.

7. If we run the current scene with the emulator controller (or build to the phone) we will only see a white dot moving around the scene, but we won't be able to see the controller or the laser. So lets position the camera and controller in the following way to set up our player view and controller position:
  <ol type="a">
    <li>Make the Main Camera a child of the DaydreamPlayer object. From now on we'll use the DaydreamPlayer to position the camera. This allows us to keep the controller and camera absolutely positioned within the player object</li>
    <li>Set the Main Camera's transform position to 0 (zero) for x, y, and z.</li>
    <li>Adjust the DaydreamPlayer player position to:
      <ul>
        <li>
          y = 1.65 to lift the camera up the average person height for both male and female
        </li>
        <li>
          z = -5 so we can comfortably view our box in the scene
        </li>
      </ul>
    </li>
    <li>Running our scene now will show us our laser pointer moving around, but we still wont be able to see the controller. This is due to the Main Camera's near clipping plane. Update this to be 0.03 (it's 0.3 by default). This will prevent the camera from cutting off objects that are close to it.</li>
    <li>*Optional* - You may notice the controller is accompanied by text indicating what the buttons are. If you're like me, this will annoy you. You can turn this off by turning off the 'Tooltips' object within the GvrControllerPointer prefab dropdown in our DaydreamPlayer</li>
  </ol>

8. Now let's get the Daydream controller to interact with the box.
  <ol type="a">
    <li>Add the BoxController.cs script from this repo to your project</li>
    <li>Add the script to the box</li>
    <li>Add an event trigger with two events:
      <ul>
        <li>
          Pointer Down - mapped to the function BoxController.PickUp()
        </li>
        <li>
          Pointer Up - mapped to the function BoxController.PutDown()
        </li>
      </ul>
    </li>
  </ol>
9. Now to get the box to respond to the controller. Select the Main Camera and add the component script GvrPointerPhysicsRaycaster. This will allow the controller to raycast against objects in the scene. Now you should be able to move the box around the screen by clicking and dragging.

#### Create prefabs of our Daydream player

At this point we have our Daydream player, which we could use in any project. Make it a prefab by:
1. Creating a prefab object in the project view (right click -> create -> prefab -> give the prefab a name)
2. Drag the DaydreamPlayer object from the Hierarchy panel onto the prefab. Done.

#### Daydream controller to work with UI elements

Lets set up a few boxes to throw around and add a button to reset their positions. First lets create some boxes.

1. Repeat the previous step of `Create prefabs of our Daydream player` with the box
2. Create an empty game object labelled Boxes
3. Create as many boxes as you like as children to the Boxes object and place them with at least a little bit of space between each one.

Now that we have some boxes, let's set up our UI button.

1. Add a UI Button the scene, label it 'ResetButton' and update the button text to say 'Reset'.
2. Update the buttons parent object 'Canvas' to have a render mode of world space.
3. Position the canvas so that it is in front of the camera and the button can be seen.
4. Attach the script BoxesManager from this repo to the Canvas object.
5. In the BoxesManager script component seen in the Canvas's inspector, add each box to the Boxes list.
6. On the button, add the script from this repo ButtonController to the button.
7. Add an OnClick() event to the button and map it to ButtonController.OnClick()

At this point if you run you're scene, you'll have a button that will be displayed when pressing the controllers menu button, however you'll notice that the pointer isn't interacting with the UI button when it's clicked. This is because UI elements cannot be reached with a physics raycaster. Instead, follow this step to set up a graphic raycaster instead.

1. On the UI Canvas, add the script "GvrPointerGraphicRaycaster".

## Daydream controller emulator setup (requires android phone)
1. Download the apk (android app file) from here: https://developers.google.com/vr/daydream/controller-emulator
2. Copy the apk file to your android phone. You can use the android file transfer tool for copying files to and from you pc/mac: https://www.android.com/filetransfer/
3. Locate the apk on your android device and open it, which will install it.
4. Connect the phone to WiFi
5. Wait for an IP address to be displayed in the controller emulator app (may require restarting the app if WiFi was disabled when the app was opened)
6. In Unity, locate the EmulatorConfig.cs file and update the IP address for the WIFI_SERVER_IP to match what is in the controller emulator.
7. In the inspector for GvrControllerMain (inside DaydreamPlayer), set the emulator connection mode to WiFi

## Building to android device
1. Plug in Daydream ready phone (requires android 7.0 or above)
2. File -> Build -> Build & Run. This will package your scene into an apk, transfer it to your phone, and run the app on your phone.

## Profiling tool to analyze performance
1. In build settings select Development Build and Autoconnect Profiler
