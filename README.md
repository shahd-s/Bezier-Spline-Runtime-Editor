# Bezier-Spline-Runtime-Editor 

This is a tool that I have created that allows you to create and edit cubic or quadtratic Bezier curves at runtime in Unity. It uses a LineRenderer and draws several conjoined Bezier segments upon mouse click. It gives you two (cubic), or one (quadtratic), control points.  I have generated these by considering the two end points of a segment to make up an equaliatiral triangle, of which the two control points are the third vertex. You can drag around these around to alter the corresponding Bezier segment. Currently, the choice of segments to be cubic or quadratic is determined by the boolean variable "cubic". In the future, I may the ability to decide between these as as an editor feature. 

## Screenshots
### Drawing a heart using cubic Beziers: [(Try it)](https://shahd-s.itch.io/cubicbezier?secret=R1e0bGWq0pbWPMBX8gPl3HHBR0o)
<img width="797" alt="screen shot 2019-02-25 at 11 59 17 pm" src="https://user-images.githubusercontent.com/18424537/53371669-910e0b00-3959-11e9-90c6-56f626b522dd.png">

### Quadtratic: [(Try it)](https://shahd-s.itch.io/quadratic-bezier?secret=cAcBfijaPJ3aJ0zcAm9QViNgMLM)
<img width="777" alt="screen shot 2019-02-26 at 12 00 30 am" src="https://user-images.githubusercontent.com/18424537/53371750-c1ee4000-3959-11e9-99ef-36bafcff0249.png">
