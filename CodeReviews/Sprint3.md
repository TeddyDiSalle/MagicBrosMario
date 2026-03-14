# Sprint2 Code Review

## Readability Review by Vincent Do

[MovingPlatform_Size1.cs](../Source/Items/MovingPlatform_Size1.cs) by Brian Perusek

- Date: 03.13.2026
- Time: 2 minutes

### Comments

>Everything is straightforward and understandable. The platform simply moves up or down and wraps back around. Its only responsibility for collision is updating its collision box.

---

## Maintainability Review by Vincent Do

[ItemManager.cs](..\Source\Level\ItemManager.cs) by Teddy DiSalle

- Date: 03.13.2026
- Time: 2 minutes

### Comments

>Item manager does its job efficiently and is easy to understand. It simply takes in the data from the documents, and for each id it find and pairs the function to the id and stores the pair in a dictionary where it is later used to create the item at the location provided. Overall very maintainable and easy to change or debug. 

---

## Readability Review by Brian Perusek

[Class.cs](../Source/Class.cs) by Author

- Date: 03.13.2026
- Time: 2 minutes

### Comments

>

---

## Maintainability Review by Brian Perusek

[Class.cs](../Source/Class.cs) by Author

- Date: 03.13.2026
- Time: 3 minutes

### Comments

>

---

## Readability Review by Teddy DiSalle

[Class.cs](../Source/Movement/MovementController.cs) by Chuang-Yun

- Date: 03.13.2026
- Time: 4 minutes

### Comments

> Nice readable class. Very easy to use by other classes and understand how to implement into our own classes. nice bro.

---

## Maintainability Review by Teddy DiSalle

[Class.cs](../Source/MarioStates/Player.cs) by Vincent

- Date: 03.13.2026
- Time: 3 minutes

### Comments

> Lot of nice and small functions and the class delegates to other classes very well. But there are too many variables and too many functions. I think if it had more helper classes and reduce its size would increase its maintainability. Then bug fixing would be easier as well when the bugs are placed in different classes and comparmentalized.

---

## Readability Review by Chuang-Yun Huang

[Level1.cs](../Source/Level/Level1.cs) by Teddy

- Date: 03.13.2026
- Time: 3 minutes

### Comments

> Although this code is really different from the type of code that I write, I can still understand it easily. He only uses simple c sharp features and I think this works really well in a group. Great job!

---

## Maintainability Review by Chuang-Yun Huang

[ItemFactory.cs](../Source/Items/ItemFactory.cs) by Brian

- Date: 03.13.2026
- Time: 4 minutes

### Comments

> The item factory is a great class to have. Since blocks and player can spawn items, it simplifies the logic a lot for me and Vincent when creating the items since we don't have to update our code when Brian updates its constructor. Most useful class in the project!

---

## Readability Review by Roshan Ramamurthy

[Camera.cs](../Source/Camera/Camaera.cs) by Chuang-Yun

- Date: 03.13.2026
- Time: 2 minutes

### Comments

> The class is well documented with XML comments explaining the camera's purpose and how the Position property works, which makes it easy to understand at a glance. The ShouldDraw method is clean and straightforward for culling off-screen sprites. One minor issue is the use of field keyword in the singleton property which is a newer C# feature that might confuse some readers. Overall very readable. 9 out of 10.

---

## Maintainability Review by Roshan Ramamurthy

[BigMarioJumpState.cs](../Source/MarioStates/BigMarioJumpState.cs) by Vincent

- Date: 03.13.2026
- Time: 5 minutes

### Comments

> This class handles Big Mario's jump state and follows the state pattern cleanly with clear transitions to other states like crouch, idle, and power-up variants. The sprite management and StateChangePrep cleanup is well organized. One small concern is that jump height is controlled by counting method calls rather than time, which could behave differently at varying frame rates. Overall solid implementation. 8 out of 10.
