## Core Tenets

1. My primary concern was keeping the logic of the single-player environment as
isolated as possible, since the instructions stated: "should have no enabled
networked object components." Thus, I wanted to ensure network functionality was
purely additive and I chose the decorator pattern to do so. Normally I would do
this with inherited method overrides, but I didn't think it was feasible because
Photon Fusion requires NetworkBehaviour as a base class. Instead I exposed some
relevant actions and simply overwrote them as needed.

2. Loose coupling is always a goal no matter what I write, so I ensured that
there was a proper interface wherever cross-communication was desired. The only
exception to this was the aforementioned decorators, for two reasons. First,
network solutions are often quite varied so the odds of reusing a decorator in
the future are minimal. Second, it would've added more bloat to the interfaces.

3. I planned to take more of an event-based approach as opposed to using direct
references. You can see this in the use of the new Input System, but primarily
in the dispatches system. In my past multiplayer work, it was very beneficial
to have an adapter layer between network events and the rest of the game. Sadly
I only needed to subscribe to one event, but it's very scalable. The dispatcher
system also provides its own ecosystem (as you can have as many as you want) as
well as type-safety through class parameters (as opposed to having to manually
write custom events, custom delegates, and custom event args).