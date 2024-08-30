# RazorMinesweeper

The perfect component to add to ANY business application.
(It's just minesweeper.)

## How to use
To use the package simply add a nuget reference to the library [found here](https://www.nuget.org/packages/BlazorMinesweeper/) and add the reference *@using BlazorMinesweeper.Components* in your _Imports.razor file. 

Then you are all set to create the Minesweeper components in your app!

The Minesweeper component exposes an asynchronous method "StartGame" which starts the game, nothing will be rendered before you call this method.

## CSS
This library comes prepacked with a basic CSS package. You need to reference the CSS file in your App.razor for it to work. Use the following syntax
``` html
<link href="_content/BlazorMinesweeper/css/minesweeper.css" rel="stylesheet" />
```
You can also add your own stylesheet with the same classes that exist in the css file! :)

## Events

### OnStart
Trigger: When the StartGame method and before the game is generated.\
Out: -

### OnGameEnd
Trigger: When the game ends.\
Out: boolean, true if won and false if loss.

### OnTileLeftClick
Trigger: When you left click on a hidden minesweeper tile.\
Out: -

### OnTileRightClick
Trigger: When you right click on a hidden minesweeper tile.\
Out: The tilestate. (enum: None > Flag > Question > None.)

## Properties

### Width
Width of the minesweeper board.\
Default: 30

### Height 
Height of the minesweeper board.\
Default: 16

### MineCount
Amount of mines on the minesweeper board.\
Default: 99

### IgnoreDefaultGameOver
Boolean for displaying a simple box when you win/lose the game, includes retry button.\
Default: false

### TileSize
Size of the tiles in px.\
Default: 20

## Exceptions

The minesweeper component **will** throw an exception when: 
  - You try to set the size of the board to smaller than 50 tiles.
  - You try to set the total amount of mines to zero or a negative number.
  - You try to set the amount of mines to be larger or equal to half the size of the board.




