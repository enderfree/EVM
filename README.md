# Disclaimer
This mod is written incompatible with [Predators Swallow](https://steamcommunity.com/workshop/filedetails/?id=3163169357) as it now includes it, but having both should cause no issue.

# Overview
This is mostly a library but it also includes Predators Swallow now, which makes so if they can, predators will swallow their prey whole. Maw size and if this feature is active are configurable. 

I was tired of adding RedMattis' [Lamia mod](https://steamcommunity.com/workshop/filedetails/?id=2908225858) as a dependency to mods that should not have any (except Harmony maybe). RedMattis' mods are amazing, but his Engulf gene is a gene, locking it to Biotech and it currently only allows Fatal and Healing. Mine should allow custom logic, although only Fatal was tested yet. 

This mod is still in beta. 

# How does this mod work?
Jaw
- ~~Added field mawSize. This is used to see if you can swallow this creature whole. (there is an option to ignore this part)~~
This was removed to confugure like from [Predators Swallow](https://steamcommunity.com/workshop/filedetails/?id=3163169357)

Stomach<br/>
Added the following fields.
- baseDamage: damage dealt per digestion tick = baseDamage * digestionEfficiancy * metabolism level
- digestionEfficiancy: damage dealt per digestion tick = baseDamage * digestionEfficiancy * metabolism level
- comfort: for those who are not being digested, how good of a bed is this place?
- deadline: time before food move to the next stage of your digestive track
- grantsNutrition: does this stomach feed you?
- nutritionCost: does this stomach takes nutrition rather than feeding you?
- digestionWorker: logic used
- digestionDamageType: type of damage your digestion applies
- isTimedStage: will the prey move to the next stage after a while
- customReleaseWorker: custom condition to release preys
Added the name tag "DefaultStomach" to the stomach, hoping to help crossmod compatibility

Body<br/>
Added a default digestive track to the human body.

# Digestive Track?
A body can have multiple digestive tracks. Each must have a Purpose and a list of the body parts a prey must go through before being freed. All those body parts must have the fields mentioned in Stomach or they will default. 

If you have the debug options activated, when you right click on a pawn while having one of your pawns selected, you have the option of in which digestive track you want to send them, while if you right click your colonist after, you can regurgitate. 

A defName field was added to let your jobs reference a specific tract.

# Digestion Worker
I made the following digestion worker so far.
- DigestionWorker_Fatal
- DigestionWorker_Heal
- DigestionWorker_HealScars
- DigestionWorker_JoyPred
- DigestionWorker_JoyPrey
- DigestionWorker_Mend
- DigestionWorker_Regenerate
- DigestionWorker_Safe
- DigestionWorker_Tend

If you want to make your own, add this mod as a library and implement DigestionWorker in your class. 

# Biotech
Biotech genes don't add abilities yet, but work well with the debug options. They add a digestive tract that only contains one stomach of the specified type. So far, here are the options:
- Safe (disable digestion)
- Enjoy Preysence (Disable digestion and grants recreation to the pred)
- Relaxing (Disable digestion and grants recreation to the prey)
- Prison (disable digestion, but never release (should probably change how it works))
- Tending (digestion tend to wounds (opi))
- Heal (digestion heal (very opi))
- Heal+ (digestion tend + heal + heal scars (very opi))
- Restoring (digestion tend + heal + heal scars + restore missing body parts (very opi))

# Plans for the future
- Snacks can be seen in the portrait bar and interacted with as if carried
- More digestion workers (someone asked for prison stuff)
- A tutorial

# Special Thanks to
- Notnyna (made an amazing [fork](https://github.com/Notnyna/EVM) that inspired the Biotech update)
- Eragon (helped me fix the bug that delayed the Biotech update for so long!)
