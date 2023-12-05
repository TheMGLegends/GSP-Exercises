// Fill out your copyright notice in the Description page of Project Settings.


#include "MyMaterialChangeActor.h"
#include "Components/MeshComponent.h"

// Sets default values
AMyMaterialChangeActor::AMyMaterialChangeActor()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AMyMaterialChangeActor::BeginPlay()
{
	Super::BeginPlay();

	UStaticMeshComponent* MySM = FindComponentByClass<UStaticMeshComponent>();
	MySM->SetMaterial(0, MyMat);

	// Gets Reference to the attached Niagara Component
	UNiagaraComponent* MyNiagaraComp = FindComponentByClass<UNiagaraComponent>();

	// Sets the value of the "MyCol" Color Parameter. 
	MyNiagaraComp->SetColorParameter("Color", FLinearColor(0, 1, 0, 1));


	// Gets the Current Material Instance and Turns it into a Dynamic Material Instance. 
	// Its' values can now be changed.
	MyDynamicMat = MySM->CreateAndSetMaterialInstanceDynamic(0);

	// Changes the Vector Parameter of “Color" into the Color Blue.
	MyDynamicMat->SetVectorParameterValue(FName(TEXT("Color")), FLinearColor(0, 0, 1, 1));

	
}

// Called every frame
void AMyMaterialChangeActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	currentTime += DeltaTime;

	if (currentTime > time) {
		currentTime = 0;
		MyDynamicMat->SetVectorParameterValue(FName(TEXT("Color")), FLinearColor(FMath::RandRange(0, 1), FMath::RandRange(0, 1), FMath::RandRange(0, 1), FMath::RandRange(0, 1)));
	}
}

