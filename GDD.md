## Game Design

#### 

#### Idea:

You are a **babysitter**. You come out to the kitchen to view the sticky note with list of tasks to complete for the "day". You only have a certain amount of energy, and each task uses energy (as well as little actions). Once completed all tasks the game fades out and you are paid $X - the day is done. You fade back in and repeat. If the baby starts crying, your energy depletes quicker and you need to tend to it quickly while still hoping to complete all the tasks for the day. If you don't complete them all (or any) you receive less (or no) money. You use money to buy more nappies and wipes.

**Noises can wake baby**. Baby has a "bar" for how much it is hearing - each action increases this.

There is a wondering dog which will either bark if it wants to go outside or it will shit on the ground which will wake baby if smell lingers, need to clean.

While the baby is awake/crying, the screen will shake dramatically, making it difficult to complete tasks or tend to the baby. To sooth baby, either change nappy or feed (depending what it wants - determined by RNG)

Add upgrade system (increase max capacity of stations) using money?



#### Logistics:



###### The energy bar

* starts at 100
* Walking chips away 1 every few steps?
* Doing tasks (loading dishwasher, taking out laundry) uses \~10 energy
* If baby is crying, energy goes down 1 per second
* If energy reaches 0 and baby still crying - movement slows? Until baby back in crib and then fade out saying passed out
* If energy reaches 0 and baby is asleep but tasks aren't complete - fade out saying passed out



###### Tasks

* Dishwasher: open dishwasher -> click on sink to grab items as a whole -> click on dishwasher to load -> click dishwasher to run it -> once completed, you have 3 seconds before it **BEEPS** which can wake baby -> click dishwasher to open it -> leave to dry or unload?
* Washing clothes: put dirty clothes into washing machine 1 by 1 -> click washing machine to run it -> once completed, you have 3 seconds before it **BEEPS** which can wake baby -> click washing machine to grab "pack" of clothes -> click dryer to load in there -> click dryer to run dryer -> once completed, you have 3 seconds before it **BEEPS** which can wake baby -> click dryer to grab "pack" of clothes -> restock clothes drawer
* Take dog out: click on dog to follow -> click on door -> fade out -> fade in
* Clean dog shit: click to pickup, click bin to bin it
* Empty bin: Click on bin to remove bag -> click door to throw bag outside -> click on kitchen drawer to get new bag -> click on bin to replace



###### Noises

* Baby has "noise level" bar
* Each task/action increases this by \~5 at the point of you clicking to do something
* If a machine **BEEPS** it increases by \~10
* Dog bark increases by \~10
* Dropping an item increases by \~15
* The bar goes down \~1 per second if no noises happening (ignores walking)
* If it exceeds \~20 then baby wakes up



###### Tending to baby

* Feed: prepare bottle in kitchen with formula, microwave, then feed baby.

Potential feature:

When you start feeding baby, a bar appears, from a yellow zone (baby still hungry) it slowly builds up

It speeds up exponentially toward a small green zone and you need to stop feeding in that zone to succeed.

If you miss the green zone, you go to the red zone and baby vomits.

If you stop before the green zone, baby it still hungry and you need to feed again (in which case green zone will be closer)



* Change: place baby on change table -> remove clothes -> put clothes in wash -> remove nappy -> put nappy in nappy bin -> get a wipe -> wipe baby X times (rand number 1-4) -> get fresh nappy -> click baby to put on nappy -> get fresh clothes -> click baby to put on clothes -> pickup baby and put back down in cot

If drops dirty nappy on ground instead of using bin, room will be stinky and baby can't sleep

