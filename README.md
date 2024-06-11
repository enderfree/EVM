# Disclaimer
This mod is written incompatible with [Predators Swallow](https://steamcommunity.com/workshop/filedetails/?id=3163169357) as it now includes it, but having both should cause no issue.

# Overview
This is mostly  a library mod for vore. I was tired of adding RedMattis' [Lamia mod](https://steamcommunity.com/workshop/filedetails/?id=2908225858) as a dependency to mods that should not have any (except Harmony maybe). 

RedMattis' mods are amazing, but his Engulf gene is a gene, locking it to Biotech and it currently only allows Fatal and Healing. Mine should allow custom logic, although only Fatal was tested yet. 

This mod adds no features by default, but has vore options that can be turned on so you can command your pawns to vore. Vore is not automatic, triggers for vore might be made later, but not as part of this mod. This mod's purpose is to handle any vore logic thrown at it. 

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
Added the name tag "DefaultStomach" to the stomach, hoping to help crossmod compatibility

Body<br/>
Added a default digestive track to the human body.

# Digestive Track?
A body can have multiple digestive tracks. Each must have a Purpose and a list of the body parts a prey must go through before being freed. All those body parts must have the fields mentioned in Stomach or they will default. 

If you have the vore options activated, when you right click on a pawn while having one of your pawns selected, you have the option of in which digestive track you want to send them, while if you right click your colonist after, you can regurgitate. 

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

# Plans for the future
- Make a family friendly version
- Biotech features
- A tutorial
