MODULE MainModule

	! test01_001

	! variables
	TASK PERS tooldata drill := [FALSE, [[300,-260,280], [0,0,0,1]], [1,[0,0,0.001],[1,0,0,0],0,0,0]];
	TASK PERS wobjdata block := [TRUE, TRUE, "", [[0,0,150], [1,0,0,0]], [[0,0,0], [1,0,0,0]]];
	TASK PERS speeddata rate := [3, 500, 5000, 1000];

	! targets
	VAR jointtarget j0 := [[-90,0,0,90,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR jointtarget j1 := [[-41,0,0,0,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p00 := [[0,0,120], [0,1,0,0], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p01 := [[0,0,50], [0,1,0,0], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p02 := [[0,0,10], [0,1,0,0], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p10 := [[84.8528137423857,84.8528137423857,0], [0.653281482438188,0.653281482438188,-0.270598050073099,-0.270598050073099], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p11 := [[35.3553390593274,35.3553390593274,0], [0.653281482438188,0.653281482438188,-0.270598050073099,-0.270598050073099], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p12 := [[7.07106781186548,7.07106781186547,0], [0.653281482438188,0.653281482438188,-0.270598050073099,-0.270598050073099], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p20 := [[2.20429143688028E-14,120,1.34973922628168E-30], [0.707106781186548,0.707106781186547,-8.65934606862825E-17,-4.32956578745078E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p21 := [[9.18454765366783E-15,50,5.62391344284032E-31], [0.707106781186548,0.707106781186547,-8.65934606862825E-17,-4.32956578745078E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p22 := [[1.83690953073357E-15,10,1.12478268856806E-31], [0.707106781186548,0.707106781186547,-8.65934606862825E-17,-4.32956578745078E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p30 := [[-84.8528137423857,84.8528137423857,-4.71027737605132E-15], [0.653281482438188,0.653281482438188,0.270598050073098,0.270598050073098], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p31 := [[-35.3553390593274,35.3553390593274,-1.96261557335472E-15], [0.653281482438188,0.653281482438188,0.270598050073098,0.270598050073098], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p32 := [[-7.07106781186547,7.07106781186548,-3.92523114670943E-16], [0.653281482438188,0.653281482438188,0.270598050073098,0.270598050073098], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p40 := [[-120,0,0], [0.5,0.5,0.5,0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p41 := [[-50,0,0], [0.5,0.5,0.5,0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p42 := [[-10,0,0], [0.5,0.5,0.5,0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p50 := [[-84.8528137423857,-84.8528137423857,4.71027737605133E-15], [0.270598050073098,0.270598050073099,0.653281482438188,0.653281482438188], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p51 := [[-35.3553390593274,-35.3553390593274,1.96261557335472E-15], [0.270598050073098,0.270598050073099,0.653281482438188,0.653281482438188], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p52 := [[-7.07106781186548,-7.07106781186548,3.92523114670944E-16], [0.270598050073098,0.270598050073099,0.653281482438188,0.653281482438188], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p60 := [[-7.34763812293426E-15,-120,4.49913075427226E-31], [7.14979088949037E-22,4.32970878326857E-17,0.707106781186548,0.707106781186547], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p61 := [[-3.06151588455594E-15,-50,1.87463781428011E-31], [7.14979088949037E-22,4.32970878326857E-17,0.707106781186548,0.707106781186547], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p62 := [[-6.12303176911189E-16,-10,3.74927562856021E-32], [7.14979088949037E-22,4.32970878326857E-17,0.707106781186548,0.707106781186547], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p70 := [[84.8528137423857,-84.8528137423857,4.71027737605133E-15], [0.270598050073099,0.270598050073098,-0.653281482438188,-0.653281482438188], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p71 := [[35.3553390593274,-35.3553390593274,1.96261557335472E-15], [0.270598050073099,0.270598050073098,-0.653281482438188,-0.653281482438188], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p72 := [[7.07106781186548,-7.07106781186548,3.92523114670944E-16], [0.270598050073099,0.270598050073098,-0.653281482438188,-0.653281482438188], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p80 := [[120,-1.46952762458685E-14,8.99826150854451E-31], [0.5,0.5,-0.5,-0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p81 := [[50,-6.12303176911188E-15,3.74927562856021E-31], [0.5,0.5,-0.5,-0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p82 := [[10,-1.22460635382238E-15,7.49855125712043E-32], [0.5,0.5,-0.5,-0.5], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];

	! drilling instructions
	PROC main()
		ConfL\Off;
		SingArea\Wrist;

		TPWrite("This is: test01_001");
		TPWrite("Check block and drill");
		MoveAbsJ j0, v100, z5, tool0;
		MoveAbsJ j1, v100, z5, tool0;

		TPWrite("Drilling hole 1 of 9!");
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL p01, v100, z5, drill\WObj:=block;
		MoveL p02, rate, fine, drill\WObj:=block;
		MoveL p01, rate, fine, drill\WObj:=block;
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL RelTool(p00, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p10, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 2 of 9!");
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL p11, v100, z5, drill\WObj:=block;
		MoveL p12, rate, fine, drill\WObj:=block;
		MoveL p11, rate, fine, drill\WObj:=block;
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL RelTool(p10, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p20, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 3 of 9!");
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL p21, v100, z5, drill\WObj:=block;
		MoveL p22, rate, fine, drill\WObj:=block;
		MoveL p21, rate, fine, drill\WObj:=block;
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL RelTool(p20, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p30, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 4 of 9!");
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL p31, v100, z5, drill\WObj:=block;
		MoveL p32, rate, fine, drill\WObj:=block;
		MoveL p31, rate, fine, drill\WObj:=block;
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL RelTool(p30, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p40, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 5 of 9!");
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL p41, v100, z5, drill\WObj:=block;
		MoveL p42, rate, fine, drill\WObj:=block;
		MoveL p41, rate, fine, drill\WObj:=block;
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL RelTool(p40, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p50, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 6 of 9!");
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL p51, v100, z5, drill\WObj:=block;
		MoveL p52, rate, fine, drill\WObj:=block;
		MoveL p51, rate, fine, drill\WObj:=block;
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL RelTool(p50, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p60, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 7 of 9!");
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL p61, v100, z5, drill\WObj:=block;
		MoveL p62, rate, fine, drill\WObj:=block;
		MoveL p61, rate, fine, drill\WObj:=block;
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL RelTool(p60, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p70, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 8 of 9!");
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL p71, v100, z5, drill\WObj:=block;
		MoveL p72, rate, fine, drill\WObj:=block;
		MoveL p71, rate, fine, drill\WObj:=block;
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL RelTool(p70, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p80, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 9 of 9!");
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL p81, v100, z5, drill\WObj:=block;
		MoveL p82, rate, fine, drill\WObj:=block;
		MoveL p81, rate, fine, drill\WObj:=block;
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL RelTool(p80, 0, 50, 0), v100, z5, drill\WObj:=block;

		TPWrite("Resetting axes...");
		MoveAbsJ j1, v100, z5, tool0;
		MoveAbsJ j0, v100, z5, tool0;

		Stop;
	ENDPROC

ENDMODULE