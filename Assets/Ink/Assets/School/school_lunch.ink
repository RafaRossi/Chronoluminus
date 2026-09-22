=== school_init ===
~speaker = friend_1_name
Ei, {player_name}! Aqui!

~speaker = player_name
(É o {friend_1_name}! Ele está junto com o resto do pessoal) #thought
(Combinamos de conversar sobre o grande plano da festa de hoje, melhor ver com eles o que planejaram) #thought

-> DONE

=== school_init_cont ===
~speaker = friend_2_name
{current_loop >= 2 : 
    {player_name}, meu Deus! Sua cara está horrível.
    ~speaker = friend_3_name
    É a única que ele tem, dá um desconto.
    -> school_init_answer_options
-else:
    Ei, {player_name} bem na hora.
    -> school_init_answer_options
}

->DONE

=== school_init_answer_options
~speaker = player_name
* {current_loop == 1} [Falar do Loop Temporal]
        Pessoal, acho que estou preso num loop temporal.
        ->DONE
    *[Acalmar os ânimos]
        Quietos.
        -> DONE
    ->DONE