Tyler Beaupre
Project 1: Follow My Spline!
https://youtu.be/qys6rU5Ig58

This is a standard Unity project which when played, shows a cube following a Catmull-Rom Spline and then a B-Spline afterward. As Splines are read in, their 
control points are drawn on the screen. Each transform, a travel point is placed where the cube went. It may take some time for the file to be read the first time.

The object which follows the spline is called 'Spline Traveler'
The script attached to it, 'SplineTraveler.cs', handles file reading and the logic for starting and stopping animations as well as drawing travel points.

The object which is responsible for drawing control points is called 'Spline'
The script 'SplineObj.cs' controls these objects. It initializes the mathematical Spline variable, draws the control points, and has some logic for calculating
 the traveler's position along the spline

The file 'Spline.cs' has all of the background math for the spline. It contains the control point positions, rotations, calculates the tangents at the control
 points, calculates the quaternions at each control point, and can hold pre-calculated B-Spline points.

The file 'MyQuaternion.cs' has all of the background math for quaternions. It has a number of constructors, one of which converts Euler angles to a Quaternion.
It also includes a number of basic quaternion functions like multiplication with scalars, addition of quats, subtraction, normalize, dot product, length, and inverse.
MyQuaternions also have a function to convert back to Euler rotations.

I still have some issues with Quaternions jumping. I give up. I really thought I could do it with the extra day...

Lastly, the file 'Timer.cs' has a class which holds timing information for the animation.