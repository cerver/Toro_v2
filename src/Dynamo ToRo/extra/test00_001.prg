MODULE MainModule

	! test00_001

	! variables
	TASK PERS tooldata drill := [FALSE, [[300,-260,280], [0,0,0,1]], [1,[0,0,0.001],[1,0,0,0],0,0,0]];
	TASK PERS wobjdata block := [TRUE, TRUE, "", [[0,0,150], [1,0,0,0]], [[0,0,0], [1,0,0,0]]];
	TASK PERS speeddata rate := [3, 500, 5000, 1000];

	! targets
	VAR jointtarget j0 := [[-90,0,0,90,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR jointtarget j1 := [[-41,0,0,0,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p00 := [[27.1746488194703,67.9366220486758,95.1112708681461], [0.303632748608409,0.945939451396106,-0.0386719050868208,-0.107285562970487], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p01 := [[11.3227703414459,28.3069258536149,39.6296961950609], [0.303632748608409,0.945939451396106,-0.0386719050868208,-0.107285562970487], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p02 := [[2.26455406828919,5.66138517072298,7.92593923901217], [0.303632748608409,0.945939451396106,-0.0386719050868208,-0.107285562970487], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p10 := [[-39.8409536444798,3.18727629155839,113.148308350323], [0.0359068018123806,0.976745076106272,0.131986139784214,0.165104260364762], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p11 := [[-16.6003973518666,1.32803178814933,47.1451284793011], [0.0359068018123806,0.976745076106272,0.131986139784214,0.165104260364762], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p12 := [[-3.32007947037332,0.265606357629865,9.42902569586022], [0.0359068018123806,0.976745076106272,0.131986139784214,0.165104260364762], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p20 := [[-48.654244366556,-28.6201437450329,105.894531856622], [0.0211624985538339,0.790090507707084,0.563013504012557,0.241505553943809], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p21 := [[-20.2726018193983,-11.9250598937637,44.1227216069258], [0.0211624985538339,0.790090507707084,0.563013504012557,0.241505553943809], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p22 := [[-4.05452036387967,-2.38501197875275,8.82454432138516], [0.0211624985538339,0.790090507707084,0.563013504012557,0.241505553943809], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p30 := [[27.0868388246697,-82.1945453989976,83.1285743239862], [0.0195472589188699,0.244010354309708,-0.887033261311551,-0.391470107397102], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p31 := [[11.2861828436124,-34.2477272495823,34.6369059683276], [0.0195472589188699,0.244010354309708,-0.887033261311551,-0.391470107397102], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p32 := [[2.25723656872247,-6.84954544991647,6.92738119366552], [0.0195472589188699,0.244010354309708,-0.887033261311551,-0.391470107397102], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];

	! drilling instructions
	PROC main()
		ConfL\Off;
		SingArea\Wrist;

		TPWrite("This is: test00_001");
		TPWrite("Check block and drill");
		MoveAbsJ j0, v100, z5, tool0;
		MoveAbsJ j1, v100, z5, tool0;

		TPWrite("Drilling hole 1 of 4!");
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL p01, v100, z5, drill\WObj:=block;
		MoveL p02, rate, fine, drill\WObj:=block;
		MoveL p01, rate, fine, drill\WObj:=block;
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL RelTool(p00, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p10, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 2 of 4!");
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL p11, v100, z5, drill\WObj:=block;
		MoveL p12, rate, fine, drill\WObj:=block;
		MoveL p11, rate, fine, drill\WObj:=block;
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL RelTool(p10, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p20, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 3 of 4!");
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL p21, v100, z5, drill\WObj:=block;
		MoveL p22, rate, fine, drill\WObj:=block;
		MoveL p21, rate, fine, drill\WObj:=block;
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL RelTool(p20, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p30, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 4 of 4!");
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL p31, v100, z5, drill\WObj:=block;
		MoveL p32, rate, fine, drill\WObj:=block;
		MoveL p31, rate, fine, drill\WObj:=block;
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL RelTool(p30, 0, 50, 0), v100, z5, drill\WObj:=block;

		TPWrite("Resetting axes...");
		MoveAbsJ j1, v100, z5, tool0;
		MoveAbsJ j0, v100, z5, tool0;

		Stop;
	ENDPROC

ENDMODULE