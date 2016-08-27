MODULE MainModule

	! test03_001

	! variables
	TASK PERS tooldata drill := [FALSE, [[300,-260,280], [0,0,0,1]], [1,[0,0,0.001],[1,0,0,0],0,0,0]];
	TASK PERS wobjdata block := [TRUE, TRUE, "", [[0,0,150], [1,0,0,0]], [[0,0,0], [1,0,0,0]]];
	TASK PERS speeddata rate := [3, 500, 5000, 1000];

	! targets
	VAR jointtarget j0 := [[-90,0,0,90,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR jointtarget j1 := [[-41,0,0,0,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p00 := [[-120,0,0], [0.5,0.5,0.5,0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p01 := [[-50,0,0], [0.5,0.5,0.5,0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p02 := [[-10,0,0], [0.5,0.5,0.5,0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p10 := [[-112.763114494309,4.9960036108132E-15,-41.0424171990802], [0.671010071662834,0.328989928337166,0.469846310392954,0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p11 := [[-46.9846310392954,2.08166817117217E-15,-17.1010071662834], [0.671010071662834,0.328989928337166,0.469846310392954,0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p12 := [[-9.39692620785909,4.16333634234434E-16,-3.42020143325669], [0.671010071662834,0.328989928337166,0.469846310392954,0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p20 := [[112.763114494309,-3.33066907387547E-15,-41.0424171990803], [0.671010071662834,0.328989928337166,-0.469846310392954,-0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p21 := [[46.9846310392954,-1.38777878078145E-15,-17.1010071662834], [0.671010071662834,0.328989928337166,-0.469846310392954,-0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p22 := [[9.39692620785908,-2.77555756156289E-16,-3.42020143325669], [0.671010071662834,0.328989928337166,-0.469846310392954,-0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p30 := [[120,-8.99826150854451E-31,-1.46952762458685E-14], [0.5,0.5,-0.5,-0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p31 := [[50,-3.74927562856021E-31,-6.12303176911188E-15], [0.5,0.5,-0.5,-0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p32 := [[10,-7.49855125712043E-32,-1.22460635382238E-15], [0.5,0.5,-0.5,-0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p40 := [[112.763114494309,6.66133814775094E-15,41.0424171990802], [0.328989928337166,0.671010071662834,-0.469846310392954,-0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p41 := [[46.9846310392954,2.77555756156289E-15,17.1010071662834], [0.328989928337166,0.671010071662834,-0.469846310392954,-0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p42 := [[9.39692620785909,5.55111512312578E-16,3.42020143325669], [0.328989928337166,0.671010071662834,-0.469846310392954,-0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p50 := [[91.9253331742774,9.99200722162641E-15,77.1345131623847], [0.17860619515673,0.82139380484327,-0.383022221559489,-0.383022221559489], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p51 := [[38.3022221559489,4.16333634234434E-15,32.139380484327], [0.17860619515673,0.82139380484327,-0.383022221559489,-0.383022221559489], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p52 := [[7.66044443118978,8.32667268468867E-16,6.4278760968654], [0.17860619515673,0.82139380484327,-0.383022221559489,-0.383022221559489], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p60 := [[60.0000000000001,-6.66133814775094E-15,103.923048454133], [0.0669872981077809,0.933012701892218,-0.249999999999999,-0.25], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p61 := [[25,-2.77555756156289E-15,43.3012701892219], [0.0669872981077809,0.933012701892218,-0.249999999999999,-0.25], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p62 := [[5,-5.55111512312578E-16,8.66025403784439], [0.0669872981077809,0.933012701892218,-0.249999999999999,-0.25], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p70 := [[20.8377813200316,-1.2490009027033E-15,118.176930361465], [0.00759612349389579,0.992403876506124,-0.0868240888334661,-0.086824088833467], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p71 := [[8.68240888334652,-5.20417042793042E-16,49.2403876506104], [0.00759612349389579,0.992403876506124,-0.0868240888334661,-0.086824088833467], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p72 := [[1.7364817766693,-1.04083408558608E-16,9.84807753012208], [0.00759612349389579,0.992403876506124,-0.0868240888334661,-0.086824088833467], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p80 := [[-20.8377813200316,4.16333634234434E-16,118.176930361465], [0.00759612349389579,0.992403876506124,0.086824088833467,0.086824088833467], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p81 := [[-8.6824088833465,1.73472347597681E-16,49.2403876506104], [0.00759612349389579,0.992403876506124,0.086824088833467,0.086824088833467], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p82 := [[-1.7364817766693,3.46944695195361E-17,9.84807753012208], [0.00759612349389579,0.992403876506124,0.086824088833467,0.086824088833467], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p90 := [[-60,-3.33066907387547E-15,103.923048454133], [0.0669872981077809,0.933012701892217,0.249999999999999,0.25], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p91 := [[-25,-1.38777878078145E-15,43.3012701892219], [0.0669872981077809,0.933012701892217,0.249999999999999,0.25], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p92 := [[-5,-2.77555756156289E-16,8.66025403784439], [0.0669872981077809,0.933012701892217,0.249999999999999,0.25], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p100 := [[-91.9253331742773,0,77.1345131623847], [0.17860619515673,0.82139380484327,0.383022221559489,0.383022221559489], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p101 := [[-38.3022221559489,0,32.139380484327], [0.17860619515673,0.82139380484327,0.383022221559489,0.383022221559489], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p102 := [[-7.66044443118978,0,6.4278760968654], [0.17860619515673,0.82139380484327,0.383022221559489,0.383022221559489], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p110 := [[-112.763114494309,1.66533453693773E-15,41.0424171990802], [0.328989928337166,0.671010071662834,0.469846310392954,0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p111 := [[-46.9846310392954,6.93889390390723E-16,17.1010071662834], [0.328989928337166,0.671010071662834,0.469846310392954,0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p112 := [[-9.39692620785909,1.38777878078145E-16,3.42020143325669], [0.328989928337166,0.671010071662834,0.469846310392954,0.469846310392954], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];

	! drilling instructions
	PROC main()
		ConfL\Off;
		SingArea\Wrist;

		TPWrite("This is: test03_001");
		TPWrite("Check block and drill");
		MoveAbsJ j0, v100, z5, tool0;
		MoveAbsJ j1, v100, z5, tool0;

		TPWrite("Drilling hole 1 of 12!");
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL p01, v100, z5, drill\WObj:=block;
		MoveL p02, rate, fine, drill\WObj:=block;
		MoveL p01, rate, fine, drill\WObj:=block;
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL RelTool(p00, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p10, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 2 of 12!");
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL p11, v100, z5, drill\WObj:=block;
		MoveL p12, rate, fine, drill\WObj:=block;
		MoveL p11, rate, fine, drill\WObj:=block;
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL RelTool(p10, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p20, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 3 of 12!");
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL p21, v100, z5, drill\WObj:=block;
		MoveL p22, rate, fine, drill\WObj:=block;
		MoveL p21, rate, fine, drill\WObj:=block;
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL RelTool(p20, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p30, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 4 of 12!");
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL p31, v100, z5, drill\WObj:=block;
		MoveL p32, rate, fine, drill\WObj:=block;
		MoveL p31, rate, fine, drill\WObj:=block;
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL RelTool(p30, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p40, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 5 of 12!");
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL p41, v100, z5, drill\WObj:=block;
		MoveL p42, rate, fine, drill\WObj:=block;
		MoveL p41, rate, fine, drill\WObj:=block;
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL RelTool(p40, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p50, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 6 of 12!");
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL p51, v100, z5, drill\WObj:=block;
		MoveL p52, rate, fine, drill\WObj:=block;
		MoveL p51, rate, fine, drill\WObj:=block;
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL RelTool(p50, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p60, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 7 of 12!");
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL p61, v100, z5, drill\WObj:=block;
		MoveL p62, rate, fine, drill\WObj:=block;
		MoveL p61, rate, fine, drill\WObj:=block;
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL RelTool(p60, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p70, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 8 of 12!");
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL p71, v100, z5, drill\WObj:=block;
		MoveL p72, rate, fine, drill\WObj:=block;
		MoveL p71, rate, fine, drill\WObj:=block;
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL RelTool(p70, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p80, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 9 of 12!");
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL p81, v100, z5, drill\WObj:=block;
		MoveL p82, rate, fine, drill\WObj:=block;
		MoveL p81, rate, fine, drill\WObj:=block;
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL RelTool(p80, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p90, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 10 of 12!");
		MoveL p90, v100, z30, drill\WObj:=block;
		MoveL p91, v100, z5, drill\WObj:=block;
		MoveL p92, rate, fine, drill\WObj:=block;
		MoveL p91, rate, fine, drill\WObj:=block;
		MoveL p90, v100, z30, drill\WObj:=block;
		MoveL RelTool(p90, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p100, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 11 of 12!");
		MoveL p100, v100, z30, drill\WObj:=block;
		MoveL p101, v100, z5, drill\WObj:=block;
		MoveL p102, rate, fine, drill\WObj:=block;
		MoveL p101, rate, fine, drill\WObj:=block;
		MoveL p100, v100, z30, drill\WObj:=block;
		MoveL RelTool(p100, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p110, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 12 of 12!");
		MoveL p110, v100, z30, drill\WObj:=block;
		MoveL p111, v100, z5, drill\WObj:=block;
		MoveL p112, rate, fine, drill\WObj:=block;
		MoveL p111, rate, fine, drill\WObj:=block;
		MoveL p110, v100, z30, drill\WObj:=block;
		MoveL RelTool(p110, 0, 50, 0), v100, z5, drill\WObj:=block;

		TPWrite("Resetting axes...");
		MoveAbsJ j1, v100, z5, tool0;
		MoveAbsJ j0, v100, z5, tool0;

		Stop;
	ENDPROC

ENDMODULE