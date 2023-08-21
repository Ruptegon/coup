# coup
Digital adaptation of Coup card game. Made in 3 days in Unity as part of recruiting project.
Due to rushed development not everything in the code is perfect but it gives some kind of insight on how I code and think.
As visual side of the project was not rated in the task, it did not get enough attention from me (as one can see on the screen).

For rules on how to play Coup check: https://www.ultraboardgames.com/coup/game-rules.php

This implementation supports simultaneous turns, which means every player can declare to counter or challenge without waiting for decision of the players before him. Because of that, player sometimes has limited time to decide wheter to challenge or counter other player's actions. Keep track of the action log at the top of the screen to not miss any developments.

Game suffers from some edge case issues I have not time to fix yet. You can check them in [Known issues](#Known-issues) section of this readme. If you find any new bugs, let me know! :D

Have fun!

## Basic information
- Unity version used: 2022.2.1f1
- Game language: English only

## How to start the application
1. Install Unity Engine version 2022.2.1f1. You can find it in the download archive: https://unity.com/releases/editor/archive
2. Open project in Unity
3. Open Init scene
4. Press play button in the editor

If you'd rather start the application from the build, please let me know and I will prepare it for you.

## Gameplay UI overview
![CoupUIMap](https://github.com/Ruptegon/coup/assets/43939696/069cb7b4-da60-4640-9d4b-8cbe5c9abbe1)
1. Player names. Human player name is in the bottom left corner.
2. Coins count of each player.
3. AI player influence. If it's white it's not revealed. Once revealed characters are represented by following colors:
 - Contessa: Red
 - Assassin: Black
 - Captain: Blue
 - Duke: Purple
 - Ambassador: Green
4. Human player influence. It's white if not revealed, but player can move his mouse over each card to peek it's color.
5. Button for using Income action.
6. Button for using Foreig Aid action.
7. Button for using Tax action. You can use it even if you don't influence the Duke. Just don't get caught.
8. Button for using Exchange action. Same as with the Tax action, you can use it even if you don't influence required character.
9. Button for using Coup action on an enemy in the chosen column. Coup action costs 7 coins - if you don't have enough pressing button won't do anything.
10. Button for using Assassinate action on an enemy in the chosen column. Assassinate action costs 3 coins - if you don't have enough pressing button won't do anything. You can use this action even if you don't influence Assassin, at your own risk.
11. Button for using Steal action on an enemy in the chosen column. Steal action does not cost any coins. Same as with the Assasinate action, you can use it even if you don't influence required character.
12. Log detailing last interaction.


## Known issues
- Exchange action was simplified and in current version, so that it automatically replaces all non revealed cards.
- Foreign Aid action can't be countered second time if first counter was successfully challenged.
- During challenge, player can't choose to pretend that he does not have card required for action he chose.
- Player can't choose which card will be revealed during challenge against counter of Steal action. If he has both Ambassador and Captain influence leftmost influence will be revealed as a proof.
- After influence card was shown as a proof during challenge, the card is not shown to the player. It's immedietly exchanged for a new one from Court.
- Game does not give starting player 1 coin less at the begining of the game in 2 player game.
