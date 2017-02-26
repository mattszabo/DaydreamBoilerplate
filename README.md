# Google Daydream Unity Setup

## Target Audience

This guide will move fairly slowly, however those who have never used Unity before may not get the explanations required to understand each step. Therefore it is recommended that you have even a very basic understanding of:
  * Unity Editor (a basic entry level tutorial should suffice)
  * Creating 3D objects and materials
  * Prefabs
  * Event Triggers

## Prerequisites

* Unity Daydream Preview (I used 5.4.2f2-GVR13)
  * https://unity3d.com/partners/google/daydream (download link near bottom of page)
* JDK (I used 1.8, I think you can use 1.7)
  * http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html  
* Android SDK with v25 build tools
  * https://developer.android.com/studio/index.html#downloads (Look for section 'Get just the command line tools')
* Latest version of Google Daydream SDK for Unity (v1.20 at time of writing)
  * https://developers.google.com/vr/unity/download#google-vr-sdk-for-unity

## Setting up Unity for Daydream dev

To get started with the Daydream setup, we first need to set up Unity with the GoogleVR SDK and configure it to package your project and deploy it to a Daydream ready device. To do this we also need to add the SDK paths for Java and android.

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

## Daydream boilerplate

Daydream apps make use of a VR headset, which takes an android phone that displays a scene in a stereoscopic view, and a Daydream controller. Here we'll set up Unity to create that stereoscopic view, add controller support, and also set up an emulator on an android device to act as a controller within Unity so you can test your progress in the editor and not have to build to a device to view each change you make.

#### Optional steps

These steps are optional and are intended to help demonstrate what the various Daydream components and scripts allow us to do.

1. Create a plane, which will act as the ground
2. Create a cube that will be set up as a box that you'll use to interact with with the Daydream controller
  * Place it above the ground plane
  * Add a rigid body component to it to enable gravity
3. Add materials to the box and ground plane and label them accordingly

#### Finally some Daydream setup

1. Create an empty game object and label it DaydreamPlayer. This will hold all our Daydream specific objects. At the end of this guide you should be able to make a prefab out of this object to use as the Daydream player which could be switched out with a different VR config (e.g. HTC Vive, which works similar to Daydream)
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
6. To set up the emulator controller so we can run our scene in the Unity Editor with expected results, follow the steps for `Daydream controller emulator setup`. Alternatively, you can build to a Daydream ready device and use a real controller with the steps for `Building to android device`

7. If we run the scene with the emulator controller set up (or build to the phone) we will see a white pointer moving around the scene, but we won't be able to see the controller or the laser. So lets position the camera to fix this with the following steps:
  <ol type="a">
    <li>Make the Main Camera a child of the DaydreamPlayer object. From now on we'll use the DaydreamPlayer to position the camera. This allows us to keep the controller and camera absolutely positioned within the DaydreamPlayer</li>
    <li>Set the Main Camera's transform position to 0 (zero) for x, y, and z.</li>
    <li>Adjust the DaydreamPlayer player position to be y=1.65, z=-5 so we can view our box in the scene</li>
    <li>Running our scene now will show us our laser pointer moving around, but we still wont be able to see the controller. This is due to the Main Camera's near clipping plane. Update this to be 0.03 (it's 0.3 by default). This will prevent the camera from cutting off objects that are close to it.</li>
    <li>*Optional* - You may notice the controller is accompanied by text indicating what the buttons are. If you're like me, this will annoy you. You can turn this off by turning off the 'Tooltips' object within the GvrControllerPointer prefab dropdown in our DaydreamPlayer</li>
  </ol>

8. Now let's get the Daydream controller to interact with the box.
  <ol type="a">
    <li>Add the BoxController.cs script from this repo to your project</li>
    <li>Add the script to the box</li>
    <li>Add an event trigger with two events: pointer down and pointer up mapped to the functions BoxController.PickUp() and BoxController.PutDown() respectively </li>
  </ol>
  Running the scene now, you should notice that the box isn't responding to the controller. That's because we're missing a physics raycaster that will allow the laser to collide with objects.
9. Select the Main Camera and add the component script GvrPointerPhysicsRaycaster. This will allow the controller to raycast against objects in the scene. Now you should be able to move the box around the screen by clicking and dragging.

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
