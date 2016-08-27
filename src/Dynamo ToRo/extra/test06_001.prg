MODULE MainModule

	! test06_001

	! targets
	VAR jointtarget j0 := [[-90,0,0,90,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR jointtarget j1 := [[-41,0,0,0,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p00 := [[0,-120,0], [2.16489014058873E-17,2.16489014058873E-17,0.707106781186547,0.707106781186548], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p01 := [[0,-50,0], [2.16489014058873E-17,2.16489014058873E-17,0.707106781186547,0.707106781186548], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p02 := [[0,-10,0], [2.16489014058873E-17,2.16489014058873E-17,0.707106781186547,0.707106781186548], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p10 := [[-51.9615242270663,-103.923048454133,30], [0.107104410158185,0.224430221803766,0.758044243788653,0.602933367234446], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p11 := [[-21.650635094611,-43.3012701892219,12.5], [0.107104410158185,0.224430221803766,0.758044243788653,0.602933367234446], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p12 := [[-4.33012701892219,-8.66025403784439,2.5], [0.107104410158185,0.224430221803766,0.758044243788653,0.602933367234446], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p20 := [[-51.9615242270663,-60,90], [0.0353840878475001,0.538529698075788,0.764843620807813,0.35177829143823], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p21 := [[-21.650635094611,-25,37.5], [0.0353840878475001,0.538529698075788,0.764843620807813,0.35177829143823], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p22 := [[-4.3301270189222,-5,7.5], [0.0353840878475001,0.538529698075788,0.764843620807813,0.35177829143823], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p30 := [[-7.34788079488412E-15,-7.34788079488412E-15,120], [3.06161699786838E-17,1,-9.37349864163661E-34,3.06161699786838E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p31 := [[-3.06161699786838E-15,-3.06161699786838E-15,50], [3.06161699786838E-17,1,-9.37349864163661E-34,3.06161699786838E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p32 := [[-6.12323399573677E-16,-6.12323399573677E-16,10], [3.06161699786838E-17,1,-9.37349864163661E-34,3.06161699786838E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p40 := [[51.9615242270663,60,90], [0.289083716073093,0.930717875776759,-0.0936174968133482,-0.203545093533033], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p41 := [[21.650635094611,25,37.5], [0.289083716073093,0.930717875776759,-0.0936174968133482,-0.203545093533033], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p42 := [[4.33012701892219,5,7.5], [0.289083716073093,0.930717875776759,-0.0936174968133482,-0.203545093533033], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p50 := [[51.9615242270663,103.923048454133,30], [0.58717854637637,0.778383630057257,-0.138271198949339,-0.173842902286325], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p51 := [[21.650635094611,43.3012701892219,12.5], [0.58717854637637,0.778383630057257,-0.138271198949339,-0.173842902286325], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p52 := [[4.33012701892219,8.66025403784439,2.5], [0.58717854637637,0.778383630057257,-0.138271198949339,-0.173842902286325], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p60 := [[1.46957615897682E-14,120,3.85808426417591E-30], [0.707106781186548,0.707106781186547,-6.4946704217662E-17,-2.16489014058873E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p61 := [[6.12323399573676E-15,50,1.6075351100733E-30], [0.707106781186548,0.707106781186547,-6.4946704217662E-17,-2.16489014058873E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p62 := [[1.22464679914735E-15,10,3.21507022014659E-31], [0.707106781186548,0.707106781186547,-6.4946704217662E-17,-2.16489014058873E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p70 := [[-51.9615242270663,103.923048454133,30], [0.58717854637637,0.778383630057257,0.13827119894934,0.173842902286325], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p71 := [[-21.650635094611,43.3012701892219,12.5], [0.58717854637637,0.778383630057257,0.13827119894934,0.173842902286325], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p72 := [[-4.33012701892219,8.66025403784439,2.5], [0.58717854637637,0.778383630057257,0.13827119894934,0.173842902286325], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p80 := [[-51.9615242270663,60.0000000000001,90], [0.289083716073094,0.930717875776759,0.0936174968133484,0.203545093533033], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p81 := [[-21.650635094611,25,37.5], [0.289083716073094,0.930717875776759,0.0936174968133484,0.203545093533033], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p82 := [[-4.3301270189222,5,7.5], [0.289083716073094,0.930717875776759,0.0936174968133484,0.203545093533033], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p90 := [[51.9615242270663,-60,90], [0.0353840878475001,0.538529698075787,-0.764843620807812,-0.351778291438231], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p91 := [[21.650635094611,-25,37.5], [0.0353840878475001,0.538529698075787,-0.764843620807812,-0.351778291438231], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p92 := [[4.33012701892219,-5,7.5], [0.0353840878475001,0.538529698075787,-0.764843620807812,-0.351778291438231], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p100 := [[51.9615242270663,-103.923048454133,30.0000000000001], [0.107104410158185,0.224430221803767,-0.758044243788655,-0.602933367234447], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p101 := [[21.650635094611,-43.3012701892219,12.5], [0.107104410158185,0.224430221803767,-0.758044243788655,-0.602933367234447], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p102 := [[4.3301270189222,-8.66025403784439,2.5], [0.107104410158185,0.224430221803767,-0.758044243788655,-0.602933367234447], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];

	! drilling instructions
	PROC main()
		ConfL\Off;
		SingArea\Wrist;

		TPWrite("This is: test06_001");
		TPWrite("Check block and drill");
		MoveAbsJ j0, v100, z5, tool0;
		MoveAbsJ j1, v100, z5, tool0;

		TPWrite("Drilling hole 1 of 11!");
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL p01, v100, z5, drill\WObj:=block;
		MoveL p02, rate, fine, drill\WObj:=block;
		MoveL p01, rate, fine, drill\WObj:=block;
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL RelTool(p00, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p10, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 2 of 11!");
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL p11, v100, z5, drill\WObj:=block;
		MoveL p12, rate, fine, drill\WObj:=block;
		MoveL p11, rate, fine, drill\WObj:=block;
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL RelTool(p10, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p20, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 3 of 11!");
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL p21, v100, z5, drill\WObj:=block;
		MoveL p22, rate, fine, drill\WObj:=block;
		MoveL p21, rate, fine, drill\WObj:=block;
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL RelTool(p20, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p30, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 4 of 11!");
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL p31, v100, z5, drill\WObj:=block;
		MoveL p32, rate, fine, drill\WObj:=block;
		MoveL p31, rate, fine, drill\WObj:=block;
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL RelTool(p30, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p40, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 5 of 11!");
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL p41, v100, z5, drill\WObj:=block;
		MoveL p42, rate, fine, drill\WObj:=block;
		MoveL p41, rate, fine, drill\WObj:=block;
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL RelTool(p40, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p50, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 6 of 11!");
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL p51, v100, z5, drill\WObj:=block;
		MoveL p52, rate, fine, drill\WObj:=block;
		MoveL p51, rate, fine, drill\WObj:=block;
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL RelTool(p50, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p60, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 7 of 11!");
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL p61, v100, z5, drill\WObj:=block;
		MoveL p62, rate, fine, drill\WObj:=block;
		MoveL p61, rate, fine, drill\WObj:=block;
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL RelTool(p60, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p70, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 8 of 11!");
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL p71, v100, z5, drill\WObj:=block;
		MoveL p72, rate, fine, drill\WObj:=block;
		MoveL p71, rate, fine, drill\WObj:=block;
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL RelTool(p70, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p80, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 9 of 11!");
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL p81, v100, z5, drill\WObj:=block;
		MoveL p82, rate, fine, drill\WObj:=block;
		MoveL p81, rate, fine, drill\WObj:=block;
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL RelTool(p80, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p90, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 10 of 11!");
		MoveL p90, v100, z30, drill\WObj:=block;
		MoveL p91, v100, z5, drill\WObj:=block;
		MoveL p92, rate, fine, drill\WObj:=block;
		MoveL p91, rate, fine, drill\WObj:=block;
		MoveL p90, v100, z30, drill\WObj:=block;
		MoveL RelTool(p90, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p100, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 11 of 11!");
		MoveL p100, v100, z30, drill\WObj:=block;
		MoveL p101, v100, z5, drill\WObj:=block;
		MoveL p102, rate, fine, drill\WObj:=block;
		MoveL p101, rate, fine, drill\WObj:=block;
		MoveL p100, v100, z30, drill\WObj:=block;
		MoveL RelTool(p100, 0, 50, 0), v100, z5, drill\WObj:=block;

		TPWrite("Resetting axes...");
		MoveAbsJ j1, v100, z5, tool0;
		MoveAbsJ j0, v100, z5, tool0;

		Stop;
	ENDPROC

ENDMODULE